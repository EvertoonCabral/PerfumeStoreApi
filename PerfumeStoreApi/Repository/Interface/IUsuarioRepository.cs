using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Repository.Interface;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetByEmailAsync(string email);
}