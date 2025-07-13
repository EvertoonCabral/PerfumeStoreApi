using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Repository;

public interface IItemEstoqueRepository : IRepository<ItemEstoque>
{
    
    Task<ItemEstoque?> ObterPorProdutoEEstoqueAsync(int produtoId, int estoqueId);
    Task<IEnumerable<ItemEstoque>> ObterPorProdutoAsync(int produtoId);
    Task<IEnumerable<ItemEstoque>> ObterPorEstoqueAsync(int estoqueId);
    
}