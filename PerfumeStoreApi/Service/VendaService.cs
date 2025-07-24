using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Context.Dtos;
using PerfumeStoreApi.Context.Dtos.ItemVenda;
using PerfumeStoreApi.Context.Dtos.Pagamento;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.Models.Enums;
using PerfumeStoreApi.Service.Interfaces;
using PerfumeStoreApi.UnitOfWork;

namespace PerfumeStoreApi.Service;

public class VendaService : IVendaService
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEstoqueService _estoqueService;

    public VendaService(IUnitOfWork unitOfWork, IMapper mapper, IEstoqueService estoqueService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _estoqueService = estoqueService;
    }


    public async Task ValidarEstoqueParaVendaAsync(List<CreateItemVendaRequest> itens, int estoqueId)
    {
        foreach (var item in itens)
        {
            var itemEstoque = await _estoqueService.ObterItemEstoqueAsync(item.ProdutoId, estoqueId);
            
            if (itemEstoque == null)
            {
                var produto = await _unitOfWork.ProdutoRepository.GetById(item.ProdutoId);
                throw new InvalidOperationException($"Produto '{produto?.Nome}' não possui estoque no local selecionado.");
            }

            if (itemEstoque.Quantidade < item.Quantidade)
            {
                throw new InvalidOperationException($"Quantidade insuficiente em estoque para o produto '{itemEstoque.Produto.Nome}'. Disponível: {itemEstoque.Quantidade}, Solicitado: {item.Quantidade}");
            }
        }
    }

    public async Task<VendaResponse> CriarVendaAsync(CreateVendaRequest request)
    {
        if (request.Itens == null || !request.Itens.Any())
        {
            throw new ArgumentException("A venda deve conter pelo menos um item.");
        }

        using var transaction = await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            // 1. Validar cliente
            var cliente = await _unitOfWork.ClienteRepository.GetById(request.ClienteId);
            if (cliente == null || !cliente.IsAtivo)
            {
                throw new InvalidOperationException("Cliente não encontrado ou inativo.");
            }

            // 2. Validar disponibilidade de estoque para todos os itens
            await ValidarEstoqueParaVendaAsync(request.Itens, request.EstoqueId);

            // 3. Criar a venda
            var venda = new Venda
            {
                ClienteId = request.ClienteId,
                DataVenda = DateTime.Now,
                Desconto = request.Desconto,
                Status = StatusVenda.Pendente,
                Observacoes = request.Observacoes,
                UsuarioVendedor = request.UsuarioVendedor
            };

            // 4. Adicionar itens à venda
            decimal valorBruto = 0;
            foreach (var itemRequest in request.Itens)
            {
                var produto = await _unitOfWork.ProdutoRepository.GetById(itemRequest.ProdutoId);
                if (produto == null || !produto.IsAtivo)
                {
                    throw new InvalidOperationException($"Produto ID {itemRequest.ProdutoId} não encontrado ou inativo.");
                }

                var precoUnitario = itemRequest.PrecoUnitario ?? produto.PrecoVenda;
                
                var itemVenda = new ItemVenda
                {
                    ProdutoId = itemRequest.ProdutoId,
                    Quantidade = itemRequest.Quantidade,
                    PrecoUnitario = precoUnitario
                };

                venda.Itens.Add(itemVenda);
                valorBruto += itemVenda.Subtotal;
            }

            // 5. Calcular valor total
            venda.ValorTotal = valorBruto - request.Desconto;

            if (venda.ValorTotal <= 0)
            {
                throw new InvalidOperationException("Valor total da venda deve ser maior que zero.");
            }

            // 6. Salvar venda
            _unitOfWork.VendaRepository.Create(venda);
            await _unitOfWork.CommitAsync();

            // 7. Baixar estoque para cada item
            foreach (var item in venda.Itens)
            {
                await _estoqueService.MovimentarEstoqueAsync(
                    item.ProdutoId,
                    request.EstoqueId,
                    item.Quantidade,
                    TipoMovimentacao.Saida,
                    $"Venda #{venda.Id} - {item.Quantidade}x {item.Produto?.Nome}",
                    request.UsuarioVendedor
                );
            }

            await transaction.CommitAsync();

            // 8. Retornar response
            var vendaCriada = await ObterVendaPorIdAsync(venda.Id);
            return vendaCriada!;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<VendaResponse> FinalizarVendaAsync(int vendaId, List<CreatePagamentoRequest> pagamentos)
    {
        var venda = await _unitOfWork.VendaRepository
            .GetByCondition(v => v.Id == vendaId)
            .Include(v => v.Pagamentos)
            .Include(v => v.Itens)
            .FirstOrDefaultAsync();

        if (venda == null)
        {
            throw new InvalidOperationException("Venda não encontrada.");
        }

        if (venda.Status != StatusVenda.Pendente)
        {
            throw new InvalidOperationException("Apenas vendas pendentes podem ser finalizadas.");
        }

        using var transaction = await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            decimal totalPagamentos = 0;

            // Processar cada pagamento
            foreach (var pagamentoRequest in pagamentos)
            {
                var pagamento = new Pagamento
                {
                    VendaId = vendaId,
                    DataPagamento = pagamentoRequest.DataPagamento ?? DateTime.Now,
                    ValorPago = pagamentoRequest.ValorPago,
                    FormaPagamento = pagamentoRequest.FormaPagamento,
                    Observacoes = pagamentoRequest.Observacoes,
                    
                };

                venda.Pagamentos.Add(pagamento);
                totalPagamentos += pagamento.ValorPago;
            }

            // Validar se o valor pago cobre o total da venda
            if (totalPagamentos < venda.ValorTotal)
            {
                throw new InvalidOperationException($"Valor pago ({totalPagamentos:C}) é menor que o total da venda ({venda.ValorTotal:C}).");
            }

            // Finalizar venda
            venda.Status = StatusVenda.Finalizada;

            await _unitOfWork.CommitAsync();
            await transaction.CommitAsync();

            return await ObterVendaPorIdAsync(vendaId);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<VendaResponse> CancelarVendaAsync(int vendaId, string motivo, string? usuarioResponsavel = null)
    {
        var venda = await _unitOfWork.VendaRepository
            .GetByCondition(v => v.Id == vendaId)
            .Include(v => v.Itens)
            .ThenInclude(i => i.Produto)
            .Include(v => v.Pagamentos)
            .FirstOrDefaultAsync();

        if (venda == null)
        {
            throw new InvalidOperationException("Venda não encontrada.");
        }

        if (venda.Status == StatusVenda.Cancelada)
        {
            throw new InvalidOperationException("Venda já está cancelada.");
        }

        using var transaction = await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            // Estornar estoque apenas se a venda estava finalizada
            if (venda.Status == StatusVenda.Finalizada)
            {
                foreach (var item in venda.Itens)
                {
                    // Encontrar o estoque onde foi feita a baixa (buscar pela movimentação)
                    var movimentacao = await _unitOfWork.MovimentacaoEstoqueRepository
                        .GetByCondition(m => m.Observacoes.Contains($"Venda #{vendaId}") && 
                                           m.ItemEstoque.ProdutoId == item.ProdutoId &&
                                           m.Tipo == TipoMovimentacao.Saida)
                        .Include(m => m.ItemEstoque)
                        .FirstOrDefaultAsync();

                    if (movimentacao != null)
                    {
                        await _estoqueService.MovimentarEstoqueAsync(
                            item.ProdutoId,
                            movimentacao.ItemEstoque.EstoqueId,
                            item.Quantidade,
                            TipoMovimentacao.Devolucao,
                            $"Cancelamento da venda #{vendaId} - {motivo}",
                            usuarioResponsavel
                        );
                    }
                }
            }

            // Cancelar venda
            venda.Status = StatusVenda.Cancelada;
            venda.Observacoes = "Venda Cancelada";

            await _unitOfWork.CommitAsync();
            await transaction.CommitAsync();

            return await ObterVendaPorIdAsync(vendaId);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<VendaResponse?> ObterVendaPorIdAsync(int id)
    {
        var venda = await _unitOfWork.VendaRepository
            .GetByCondition(v => v.Id == id)
            .Include(v => v.Cliente)
            .Include(v => v.Itens)
            .ThenInclude(i => i.Produto)
            .Include(v => v.Pagamentos)
            .FirstOrDefaultAsync();

        if (venda == null)
            return null;

        return _mapper.Map<VendaResponse>(venda);
    }

    public async Task<List<VendaResponse>> ObterVendasAsync(DateTime? dataInicio = null, DateTime? dataFim = null, 
        StatusVenda? status = null, int? clienteId = null)
    {
        // Aguarda o GetAll() que retorna Task<IQueryable<T>>
        var query = await _unitOfWork.VendaRepository.GetAll();

        if (dataInicio.HasValue)
            query = query.Where(v => v.DataVenda >= dataInicio.Value);

        if (dataFim.HasValue)
            query = query.Where(v => v.DataVenda <= dataFim.Value);

        if (status.HasValue)
            query = query.Where(v => v.Status == status.Value);

        if (clienteId.HasValue)
            query = query.Where(v => v.ClienteId == clienteId.Value);

        var vendas = await query
            .Include(v => v.Cliente)
            .Include(v => v.Itens)
            .ThenInclude(i => i.Produto)
            .Include(v => v.Pagamentos)
            .OrderByDescending(v => v.DataVenda)
            .ToListAsync();

        // ✅ CORRIGIDO: Mapeamento para List<VendaResponse>
        return _mapper.Map<List<VendaResponse>>(vendas);
    }
    

    
}