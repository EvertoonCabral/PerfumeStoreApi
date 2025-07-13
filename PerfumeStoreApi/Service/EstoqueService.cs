using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Data.Dtos.Movimentação;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.Models.Enums;
using PerfumeStoreApi.Service.Interfaces;
using PerfumeStoreApi.UnitOfWork;

namespace PerfumeStoreApi.Service;

public class EstoqueService : IEstoqueService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public EstoqueService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<EstoqueResponse> CriarEstoqueAsync(CreateEstoqueRequest request)
    {
        var estoqueExistente = await _unitOfWork.EstoqueRepository
            .GetByCondition(e => e.Nome.ToLower() == request.Nome.ToLower())
            .FirstOrDefaultAsync();

        if (estoqueExistente != null)
        {
            throw new InvalidOperationException("Já existe um estoque com este nome");
        }

        var novoEstoque = new Estoque
        {
            Nome = request.Nome,
            Descricao = request.Descricao,
            DataCriacao = DateTime.Now
        };

        _unitOfWork.EstoqueRepository.Create(novoEstoque);
        await _unitOfWork.CommitAsync();

        
        var movimentacao = new MovimentacaoEstoque
        {
            ItemEstoqueId = null,
            Quantidade = 0,
            Tipo = TipoMovimentacao.Criacao,
            DataMovimentacao = DateTime.Now,
            Observacoes = $"Estoque '{novoEstoque.Nome}' criado",
            UsuarioResponsavel = request.UsuarioResponsavel
        };

        _unitOfWork.MovimentacaoEstoqueRepository.Create(movimentacao);
        await _unitOfWork.CommitAsync();

        return new EstoqueResponse
        {
            Id = novoEstoque.Id,
            Nome = novoEstoque.Nome,
            Descricao = novoEstoque.Descricao,
            DataCriacao = novoEstoque.DataCriacao,
            TotalItens = 0,
            TotalProdutos = 0
        };
    }

public async Task<EstoqueResponse?> ObterEstoquePorIdAsync(int id)
{
    var estoque = await _unitOfWork.EstoqueRepository
        .GetByCondition(e => e.Id == id)
        .Include(e => e.ItensEstoque)  // Incluir os itens para cálculo
        .FirstOrDefaultAsync();
    
    if (estoque == null)
        return null;

    var response = _mapper.Map<Estoque, EstoqueResponse>(estoque);

    return response;
}

public async Task<List<EstoqueResponse>> ObterTodosEstoquesAsync()
{
    var query = await _unitOfWork.EstoqueRepository.GetAll();
    var estoques = await query
        .Include(e => e.ItensEstoque)
        .ToListAsync();

    var response = _mapper.Map<List<Estoque>, List<EstoqueResponse>>(estoques);
    
    return response;
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
    using var transaction = await _unitOfWork.BeginTransactionAsync();
    
    try
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
                    Quantidade = 0,
                    DataUltimaMovimentacao = DateTime.Now
                };
                _unitOfWork.ItemEstoqueRepository.Create(itemEstoque);
                
                // Salvar para gerar o ID
                await _unitOfWork.CommitAsync();
                
                // Recarregar o item para ter o ID correto
                itemEstoque = await ObterItemEstoqueAsync(produtoId, estoqueId);
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
            TipoMovimentacao.Transferencia => quantidadeAnterior - quantidade,
            TipoMovimentacao.Ajuste => quantidade,
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
            DataMovimentacao = DateTime.Now,
            Observacoes = observacoes,
            UsuarioResponsavel = usuarioResponsavel
        };

        _unitOfWork.MovimentacaoEstoqueRepository.Create(movimentacao);
        await _unitOfWork.CommitAsync();
        
        await transaction.CommitAsync();
        return true;
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
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