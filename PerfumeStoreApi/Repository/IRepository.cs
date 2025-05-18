namespace PerfumeStoreApi.Repository;

public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
    
    
}