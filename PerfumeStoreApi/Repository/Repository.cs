using PerfumeStoreApi.Context;

namespace PerfumeStoreApi.Repository;

public class Repository<T> : IRepository<T> where T : class 
{

    protected readonly AppDbContext _context;
    
    public Repository(AppDbContext context)
    {
        _context = context;
    }
    
    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public T? GetById(int id)
    {
        
        return _context.Set<T>().Find(id);
    }

    public T Create(T entity)
    {
        
        _context.Set<T>().Add(entity);
        return entity;
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        return entity;
        
    }

    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        return entity;
        
    }
} 
