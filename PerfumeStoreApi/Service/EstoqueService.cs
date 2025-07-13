using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.Models.Enums;
using PerfumeStoreApi.Repository;
using PerfumeStoreApi.Service.Interfaces;
using PerfumeStoreApi.UnitOfWork;

namespace PerfumeStoreApi.Service;

public class EstoqueService : IEstoqueService
{
    private readonly IUnitOfWork _unitOfWork;


    public EstoqueService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
   public async Task<ItemEstoque?> ObterItemEstoqueAsync(int produtoId, int estoqueId)
    {
        return await _unitOfWork.ItemEstoqueRepository
            .GetByCondition(ie => ie.ProdutoId == produtoId && ie.EstoqueId == estoqueId)
            .Include(ie => ie.Produto)
            .Include(ie => ie.Estoque)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ItemEstoque>> ObterEstoquePorProdutoAsync(int produtoId)
    {
        return await _unitOfWork.ItemEstoqueRepository
            .GetByCondition(ie => ie.ProdutoId == produtoId)
            .Include(ie => ie.Estoque)
            .ToListAsync();
    }

    public async Task<IEnumerable<ItemEstoque>> ObterItensPorEstoqueAsync(int estoqueId)
    {
        return await _unitOfWork.ItemEstoqueRepository
            .GetByCondition(ie => ie.EstoqueId == estoqueId)
            .Include(ie => ie.Produto)
            .ToListAsync();
    }

    public async Task<bool> MovimentarEstoqueAsync(int produtoId, int estoqueId, int quantidade, 
        TipoMovimentacao tipo, string? observacoes = null, string? usuarioResponsavel = null)
    {
        var itemEstoque = await ObterItemEstoqueAsync(produtoId, estoqueId);
        
        if (itemEstoque == null)
        {
            // Criar novo item se não existir (para entradas)
            if (tipo == TipoMovimentacao.Entrada)
            {
                itemEstoque = new ItemEstoque
                {
                    ProdutoId = produtoId,
                    EstoqueId = estoqueId,
                    Quantidade = 0
                };
                _unitOfWork.ItemEstoqueRepository.Create(itemEstoque);
                
                // Precisa fazer commit para gerar o ID antes de criar a movimentação
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new InvalidOperationException("Produto não encontrado no estoque.");
            }
        }

        var quantidadeAnterior = itemEstoque.Quantidade;
        var novaQuantidade = tipo switch
        {
            TipoMovimentacao.Entrada or TipoMovimentacao.Devolucao => quantidadeAnterior + quantidade,
            TipoMovimentacao.Saida or TipoMovimentacao.Perda => quantidadeAnterior - quantidade,
            TipoMovimentacao.Transferencia => quantidadeAnterior - quantidade, // Saída na transferência
            TipoMovimentacao.Ajuste => quantidade, // Quantidade absoluta
            _ => throw new ArgumentException("Tipo de movimentação não suportado")
        };

        if (novaQuantidade < 0)
        {
            throw new InvalidOperationException("Quantidade insuficiente em estoque.");
        }

        // Atualizar quantidade
        itemEstoque.Quantidade = novaQuantidade;
        itemEstoque.DataUltimaMovimentacao = DateTime.Now;

        // Registrar movimentação
        var movimentacao = new MovimentacaoEstoque
        {
            ItemEstoqueId = itemEstoque.Id,
            Tipo = tipo,
            Quantidade = Math.Abs(quantidade),
            QuantidadeAnterior = quantidadeAnterior,
            QuantidadePosterior = novaQuantidade,
            Observacoes = observacoes,
            UsuarioResponsavel = usuarioResponsavel
        };

        _unitOfWork.MovimentacaoEstoqueRepository.Create(movimentacao);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> TransferirEstoqueAsync(int produtoId, int estoqueOrigemId, int estoqueDestinoId, 
        int quantidade, string? observacoes = null, string? usuarioResponsavel = null)
    {
        // Verificar se há quantidade suficiente na origem
        var itemOrigem = await ObterItemEstoqueAsync(produtoId, estoqueOrigemId);
        if (itemOrigem == null || itemOrigem.Quantidade < quantidade)
        {
            throw new InvalidOperationException("Quantidade insuficiente no estoque de origem.");
        }

        // Usar transação para garantir que ambas as operações sejam executadas
        using var transaction = await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            // Realizar saída na origem
            await MovimentarEstoqueAsync(produtoId, estoqueOrigemId, quantidade, TipoMovimentacao.Transferencia, 
                $"Transferência para estoque {estoqueDestinoId}. {observacoes}", usuarioResponsavel);

            // Realizar entrada no destino
            await MovimentarEstoqueAsync(produtoId, estoqueDestinoId, quantidade, TipoMovimentacao.Entrada, 
                $"Transferência do estoque {estoqueOrigemId}. {observacoes}", usuarioResponsavel);

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<ItemEstoque>> ObterItensComEstoqueBaixoAsync()
    {
        return await _unitOfWork.ItemEstoqueRepository
            .GetByCondition(ie => ie.QuantidadeMinima.HasValue && ie.Quantidade <= ie.QuantidadeMinima)
            .Include(ie => ie.Produto)
            .Include(ie => ie.Estoque)
            .ToListAsync();
    }

    public async Task<IEnumerable<MovimentacaoEstoque>> ObterHistoricoMovimentacaoAsync(int produtoId, 
        int? estoqueId = null, DateTime? dataInicio = null, DateTime? dataFim = null)
    {
        var query = _unitOfWork.MovimentacaoEstoqueRepository
            .GetByCondition(m => m.ItemEstoque.ProdutoId == produtoId);

        if (estoqueId.HasValue)
            query = query.Where(m => m.ItemEstoque.EstoqueId == estoqueId.Value);

        if (dataInicio.HasValue)
            query = query.Where(m => m.DataMovimentacao >= dataInicio.Value);

        if (dataFim.HasValue)
            query = query.Where(m => m.DataMovimentacao <= dataFim.Value);

        return await query
            .Include(m => m.ItemEstoque)
            .ThenInclude(ie => ie.Produto)
            .Include(m => m.ItemEstoque)
            .ThenInclude(ie => ie.Estoque)
            .OrderByDescending(m => m.DataMovimentacao)
            .ToListAsync();
    }

    public async Task<int> ObterQuantidadeTotalProdutoAsync(int produtoId)
    {
        return await _unitOfWork.ItemEstoqueRepository
            .GetByCondition(ie => ie.ProdutoId == produtoId)
            .SumAsync(ie => ie.Quantidade);
    }
    
}