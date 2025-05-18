using PerfumeStoreApi.Context;
using PerfumeStoreApi.Repository;

namespace PerfumeStoreApi.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private IClienteRepository _clienteRepo;
    public AppDbContext _context { get; }


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
    public void Commit()
    {
     _context.SaveChanges();   
    }
    public void Dispose()
    {
        _context.Dispose();
    }
    
}