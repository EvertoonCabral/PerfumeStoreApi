using Microsoft.EntityFrameworkCore.Storage;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Data;
using PerfumeStoreApi.Repository;
using PerfumeStoreApi.Repository.Interface;

namespace PerfumeStoreApi.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private IProdutoRepository _produtoRepo;
    private IClienteRepository _clienteRepo;
    private IVendaRepository _vendaRepo;
    private IEstoqueRepository _estoqueRepo;
    private IItemEstoqueRepository _itemEstoqueRepo;
    private IMovimentacaoEstoqueRepository _movimentacaoEstoqueRepo;
    private readonly AppDbContext _context; 


    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IClienteRepository ClienteRepository
    {
        get
        {
            return _clienteRepo ??= new ClienteRepository(_context);
        }
     
    }
    
    public IEstoqueRepository EstoqueRepository
    {
        get
        {
            return _estoqueRepo ??= new EstoqueRepository(_context);
        }
     
    }    public IItemEstoqueRepository ItemEstoqueRepository
    {
        get
        {
            return _itemEstoqueRepo ??= new ItemEstoqueRepository(_context);
        }
     
    }public IMovimentacaoEstoqueRepository MovimentacaoEstoqueRepository
    {
        get
        {
            return _movimentacaoEstoqueRepo ??= new MovimentacaoEstoqueRepository(_context);
        }
     
    }

    public IProdutoRepository ProdutoRepository
    {
        get
        {
            return _produtoRepo ??= new ProdutoRepository(_context);
        }
    }
    
    public IVendaRepository VendaRepository
    {
        get
        {
            return _vendaRepo ??= new VendaRepository(_context);
        }
    }
    public void Commit()
    {
     _context.SaveChanges();   
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();  
        
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
    
}