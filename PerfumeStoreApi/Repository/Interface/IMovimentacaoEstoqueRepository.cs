using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Repository;

public interface IMovimentacaoEstoqueRepository : IRepository<MovimentacaoEstoque>
{
    
    Task<IEnumerable<MovimentacaoEstoque>> ObterPorProdutoAsync(int produtoId, DateTime? dataInicio = null, DateTime? dataFim = null);

    
}