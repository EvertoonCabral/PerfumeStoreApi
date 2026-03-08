using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Repository.Interface;

public interface IVendaRepository : IRepository<Venda>
{
    public IQueryable<Venda> Query();
}