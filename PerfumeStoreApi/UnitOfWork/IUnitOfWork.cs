using Microsoft.EntityFrameworkCore.Storage;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Repository;

namespace PerfumeStoreApi.UnitOfWork;

public interface IUnitOfWork
{
    IItemEstoqueRepository ItemEstoqueRepository { get; }
    IEstoqueRepository EstoqueRepository { get; }
    IMovimentacaoEstoqueRepository MovimentacaoEstoqueRepository { get; }
    IProdutoRepository ProdutoRepository { get; }
    IClienteRepository ClienteRepository { get; }
    IVendaRepository VendaRepository { get; }

    void Commit();

    Task CommitAsync();
    
    Task<IDbContextTransaction> BeginTransactionAsync();

}