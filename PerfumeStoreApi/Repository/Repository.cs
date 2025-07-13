using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Data;

namespace PerfumeStoreApi.Repository;

public class Repository<T> : IRepository<T> where T : class 
{

    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    
    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    
    public virtual async Task<IQueryable<T>> GetAll()
    {
        return await Task.FromResult(_context.Set<T>());
    }

    public async Task<T?> GetById(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
    
    public virtual IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression);
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
