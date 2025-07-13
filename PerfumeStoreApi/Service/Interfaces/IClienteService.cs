using PerfumeStoreApi.Context.Dtos;
using PerfumeStoreApi.Data.Dtos.Cliente;

namespace PerfumeStoreApi.Service.Interfaces;

public interface IClienteService
{
    Task<OperationResult<PagedResult<ClienteDto>>> GetClientesAsync(ClienteFiltroDto filtros);
    Task<OperationResult<ClienteDto>> GetClienteByIdAsync(int id);
    Task<OperationResult<ClienteDetalhesDto>> GetClienteDetalhesAsync(int id);
    Task<OperationResult<ClienteDto>> CreateClienteAsync(ClienteCreateUpdateDto clienteDto);
    Task<OperationResult<ClienteDto>> UpdateClienteAsync(int id, ClienteCreateUpdateDto clienteDto);
    Task<OperationResult<bool>> DesativarClienteAsync(int id);
    Task<OperationResult<bool>> DeleteClienteAsync(int id);
}