using PerfumeStoreApi.Data.Dtos;
using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Service.Interfaces;

public interface IAuthService
{
    Task<OperationResult<string>> RegistrarAsync(Usuario usuario, string senha);
    Task<OperationResult<string>> LoginAsync(string email, string senha);
}