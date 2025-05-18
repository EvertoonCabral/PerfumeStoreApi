using PerfumeStoreApi.Repository;

namespace PerfumeStoreApi.UnitOfWork;

public interface IUnitOfWork
{
 
    IClienteRepository ClienteRepository { get; }
    
    void Commit();
    
    
}