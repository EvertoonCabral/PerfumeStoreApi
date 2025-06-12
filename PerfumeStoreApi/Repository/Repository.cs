using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Context;

namespace PerfumeStoreApi.Repository;

public class Repository<T> : IRepository<T> where T : class 
{

    protected readonly AppDbContext _context;
    
    public Repository(AppDbContext context)
    {
        _context = context;
    }
    
    public  async Task <IEnumerable<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetById(int id)
    {
        
        return await _context.Set<T>().FindAsync(id);
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
