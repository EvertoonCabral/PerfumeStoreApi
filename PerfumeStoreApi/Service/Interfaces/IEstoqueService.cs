using Microsoft.AspNetCore.Mvc;
using PerfumeStoreApi.Data.Dtos.ItemVenda;
using PerfumeStoreApi.Data.Dtos.Movimentação;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.Models.Enums;

namespace PerfumeStoreApi.Service.Interfaces;

public interface IEstoqueService
{
    Task<ItemEstoque?> ObterItemEstoqueAsync(int produtoId, int estoqueId);
    Task<IEnumerable<ItemEstoqueResponse>> ObterEstoquePorProdutoAsync(int produtoId);
    Task<IEnumerable<ItemEstoqueResponse>> ObterItensPorEstoqueAsync(int estoqueId);

    Task<bool> MovimentarEstoqueAsync(int produtoId, int estoqueId, int quantidade, TipoMovimentacao tipo,
        string? observacoes = null, string? usuarioResponsavel = null);

    Task<bool> TransferirEstoqueAsync(int produtoId, int estoqueOrigemId, int estoqueDestinoId, int quantidade,
        string? observacoes = null, string? usuarioResponsavel = null);

    Task<IEnumerable<ItemEstoqueResponse>> ObterItensComEstoqueBaixoAsync();

    Task<IEnumerable<MovimentacaoResponse>> ObterHistoricoMovimentacaoAsync(int produtoId, int? estoqueId = null,
        DateTime? dataInicio = null, DateTime? dataFim = null);

    Task<int> ObterQuantidadeTotalProdutoAsync(int produtoId);

    Task<EstoqueResponse> CriarEstoqueAsync(CreateEstoqueRequest request);
    Task<List<EstoqueResponse>> ObterTodosEstoquesAsync();
    Task<EstoqueResponse> ObterEstoquePorIdAsync(int id);
    
}