using PerfumeStoreApi.Context;
using PerfumeStoreApi.Repository;

namespace PerfumeStoreApi.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private IProdutoRepository _produtoRepo;
    private IClienteRepository _clienteRepo;
    private IVendaRepository _vendaRepo;
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

    public void Dispose()
    {
        _context.Dispose();
    }
    
}