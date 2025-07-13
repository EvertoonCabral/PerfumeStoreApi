using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Repository;

public interface IProdutoRepository : IRepository<Produto>
{
    void Update(Produto existingEntity, Produto newValues);
    
}