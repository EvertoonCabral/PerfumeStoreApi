using Microsoft.AspNetCore.Mvc;
using PerfumeStoreApi.Context.Dtos;
using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Repository;

public interface IClienteRepository : IRepository<Cliente>
{
    public Task<ActionResult<ClienteDetalhesDto>> RetornaClienteDetalhesAsync(int id);
    public Task<Cliente?> GetByCpfAsync(string cpf); // Novo método

}