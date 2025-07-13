using PerfumeStoreApi.Models;
using PerfumeStoreApi.Models.Enums;

namespace PerfumeStoreApi.Service.Interfaces;

public interface IEstoqueService
{
    Task<ItemEstoque?> ObterItemEstoqueAsync(int produtoId, int estoqueId);
    Task<IEnumerable<ItemEstoque>> ObterEstoquePorProdutoAsync(int produtoId);
    Task<IEnumerable<ItemEstoque>> ObterItensPorEstoqueAsync(int estoqueId);
    Task<bool> MovimentarEstoqueAsync(int produtoId, int estoqueId, int quantidade, TipoMovimentacao tipo, string? observacoes = null, string? usuarioResponsavel = null);
    Task<bool> TransferirEstoqueAsync(int produtoId, int estoqueOrigemId, int estoqueDestinoId, int quantidade, string? observacoes = null, string? usuarioResponsavel = null);
    Task<IEnumerable<ItemEstoque>> ObterItensComEstoqueBaixoAsync();
    Task<IEnumerable<MovimentacaoEstoque>> ObterHistoricoMovimentacaoAsync(int produtoId, int? estoqueId = null, DateTime? dataInicio = null, DateTime? dataFim = null);
    Task<int> ObterQuantidadeTotalProdutoAsync(int produtoId);
}