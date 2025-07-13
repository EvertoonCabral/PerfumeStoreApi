using System.Linq.Expressions;

namespace PerfumeStoreApi.Repository;

public interface IRepository<T>
{
    Task <IEnumerable<T>> GetAll();
    Task<T?> GetById(int id);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
    IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);

}