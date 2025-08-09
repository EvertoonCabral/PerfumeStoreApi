using Microsoft.EntityFrameworkCore.Storage;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Repository;
using PerfumeStoreApi.Repository.Interface;

namespace PerfumeStoreApi.UnitOfWork;

public interface IUnitOfWork
{
    IItemEstoqueRepository ItemEstoqueRepository { get; }
    IEstoqueRepository EstoqueRepository { get; }
    IMovimentacaoEstoqueRepository MovimentacaoEstoqueRepository { get; }
    IProdutoRepository ProdutoRepository { get; }
    IClienteRepository ClienteRepository { get; }
    IVendaRepository VendaRepository { get; }
    IUsuarioRepository UsuarioRepository { get; }

    void Commit();

    Task CommitAsync();
    
    Task<IDbContextTransaction> BeginTransactionAsync();

}