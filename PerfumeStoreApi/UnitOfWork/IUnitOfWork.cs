using PerfumeStoreApi.Context;
using PerfumeStoreApi.Repository;

namespace PerfumeStoreApi.UnitOfWork;

public interface IUnitOfWork
{
    IProdutoRepository ProdutoRepository { get; }
    IClienteRepository ClienteRepository { get; }

    void Commit();

    Task CommitAsync();
}