using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Service.Interfaces;

public interface IAuthService
{
    Task<string> RegistrarAsync(Usuario usuario, string senha);
    Task<string?> LoginAsync(string email, string senha);
}