using AutoMapper;
using PerfumeStoreApi.Context.Dtos;
using PerfumeStoreApi.Data.Dtos;
using PerfumeStoreApi.Data.Dtos.Cliente;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.Service.Interfaces;
using PerfumeStoreApi.UnitOfWork;

namespace PerfumeStoreApi.Service;

public class ClienteService : IClienteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ClienteService> _logger;


    public ClienteService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ClienteService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }


    public async Task<OperationResult<PagedResult<ClienteDto>>> GetClientesAsync(ClienteFiltroDto filtros)
    {

        try
        {
            if (filtros.PageNumber  < 1 ) filtros.PageNumber = 1;
            if (filtros.PageSize < 1|| filtros.PageSize > 100) filtros.PageSize = 10;
            
            var clientes = await _unitOfWork.ClienteRepository.GetAll();
            var clientesDto = _mapper.Map<List<ClienteDto>>(clientes);

            var clientesPaginados = clientesDto
                .Skip((filtros.PageNumber - 1) * filtros.PageSize)
                .Take(filtros.PageSize)
                .ToList();

            var resultado = new PagedResult<ClienteDto>
            {
                Items = clientesPaginados,
                TotalCount = clientesDto.Count,
                PageNumber = filtros.PageNumber,
                PageSize = filtros.PageSize
            };
            _logger.LogInformation("Busca de clientes realizada. Total: {TotalCount}", clientesDto.Count);
            return OperationResult<PagedResult<ClienteDto>>.CreateSuccess(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar clientes");
            return OperationResult<PagedResult<ClienteDto>>.CreateFailure("Erro interno ao buscar clientes");
        }
        
    }

   public async Task<OperationResult<ClienteDto>> GetClienteByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
            {
                return OperationResult<ClienteDto>.CreateFailure("ID deve ser maior que zero");
            }

            var cliente = await _unitOfWork.ClienteRepository.GetById(id);
            if (cliente == null)
            {
                _logger.LogWarning("Cliente com ID {ClienteId} não encontrado", id);
                return OperationResult<ClienteDto>.CreateFailure($"Cliente com ID {id} não encontrado");
            }

            var clienteDto = _mapper.Map<ClienteDto>(cliente);
            return OperationResult<ClienteDto>.CreateSuccess(clienteDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar cliente {ClienteId}", id);
            return OperationResult<ClienteDto>.CreateFailure("Erro interno ao buscar cliente");
        }
    }

    public async Task<OperationResult<ClienteDetalhesDto>> GetClienteDetalhesAsync(int id)
    {
        try
        {
            if (id <= 0)
            {
                return OperationResult<ClienteDetalhesDto>.CreateFailure("ID deve ser maior que zero");
            }

            var cliente = await _unitOfWork.ClienteRepository.GetById(id);
            if (cliente == null)
            {
                return OperationResult<ClienteDetalhesDto>.CreateFailure($"Cliente com ID {id} não encontrado");
            }

            var clienteDetalhes = _mapper.Map<ClienteDetalhesDto>(cliente);
            
            return OperationResult<ClienteDetalhesDto>.CreateSuccess(clienteDetalhes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar detalhes do cliente {ClienteId}", id);
            return OperationResult<ClienteDetalhesDto>.CreateFailure("Erro interno ao buscar detalhes do cliente");
        }
    }

    public async Task<OperationResult<ClienteDto>> CreateClienteAsync(ClienteCreateUpdateDto clienteDto)
    {
        try
        {
            // Validar CPF único se informado
            if (!string.IsNullOrEmpty(clienteDto.Cpf))
            {
                var cpfValido = await ValidarCpfUnicoAsync(clienteDto.Cpf);
                if (!cpfValido.Success)
                {
                    return OperationResult<ClienteDto>.CreateFailure(cpfValido.Errors);
                }
            }

            var cliente = _mapper.Map<Cliente>(clienteDto);
            cliente.DataCadastro = DateTime.Now;

            _unitOfWork.ClienteRepository.Create(cliente);
            await _unitOfWork.CommitAsync();

            var clienteCriado = _mapper.Map<ClienteDto>(cliente);
            
            _logger.LogInformation("Cliente {ClienteId} criado com sucesso", cliente.Id);
            return OperationResult<ClienteDto>.CreateSuccess(clienteCriado, "Cliente criado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar cliente");
            return OperationResult<ClienteDto>.CreateFailure("Erro interno ao criar cliente");
        }
    }

    public async Task<OperationResult<ClienteDto>> UpdateClienteAsync(int id, ClienteCreateUpdateDto clienteDto)
    {
        try
        {
            if (id <= 0)
            {
                return OperationResult<ClienteDto>.CreateFailure("ID deve ser maior que zero");
            }

            var cliente = await _unitOfWork.ClienteRepository.GetById(id);
            if (cliente == null)
            {
                return OperationResult<ClienteDto>.CreateFailure($"Cliente com ID {id} não encontrado");
            }

            // Validar CPF único se alterado
            if (!string.IsNullOrEmpty(clienteDto.Cpf) && clienteDto.Cpf != cliente.Cpf)
            {
                var cpfValido = await ValidarCpfUnicoAsync(clienteDto.Cpf, id);
                if (!cpfValido.Success)
                {
                    return OperationResult<ClienteDto>.CreateFailure(cpfValido.Errors);
                }
            }

            _mapper.Map(clienteDto, cliente);
            await _unitOfWork.CommitAsync();

            var clienteAtualizado = _mapper.Map<ClienteDto>(cliente);
            
            _logger.LogInformation("Cliente {ClienteId} atualizado com sucesso", id);
            return OperationResult<ClienteDto>.CreateSuccess(clienteAtualizado, "Cliente atualizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar cliente {ClienteId}", id);
            return OperationResult<ClienteDto>.CreateFailure("Erro interno ao atualizar cliente");
        }
    }

    public async Task<OperationResult<bool>> DesativarClienteAsync(int id)
    {
        try
        {
            var cliente = await _unitOfWork.ClienteRepository.GetById(id);
            if (cliente == null)
            {
                return OperationResult<bool>.CreateFailure($"Cliente com ID {id} não encontrado");
            }

            if (!cliente.IsAtivo)
            {
                return OperationResult<bool>.CreateFailure("Cliente já está desativado");
            }

            cliente.IsAtivo = false;
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Cliente {ClienteId} desativado", id);
            return OperationResult<bool>.CreateSuccess(true, "Cliente desativado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao desativar cliente {ClienteId}", id);
            return OperationResult<bool>.CreateFailure("Erro interno ao desativar cliente");
        }
    }

    public async Task<OperationResult<bool>> DeleteClienteAsync(int id)
    {
        try
        {
            var cliente = await _unitOfWork.ClienteRepository.GetById(id);
            if (cliente == null)
            {
                return OperationResult<bool>.CreateFailure($"Cliente com ID {id} não encontrado");
            }

            // Verificar se tem vendas associadas
            if (cliente.Vendas.Any())
            {
                return OperationResult<bool>.CreateFailure("Não é possível excluir cliente que possui vendas. Use a opção desativar.");
            }

            _unitOfWork.ClienteRepository.Delete(cliente);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Cliente {ClienteId} excluído permanentemente", id);
            return OperationResult<bool>.CreateSuccess(true, "Cliente excluído com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir cliente {ClienteId}", id);
            return OperationResult<bool>.CreateFailure("Erro interno ao excluir cliente");
        }
    }

    public async Task<OperationResult<bool>> ValidarCpfUnicoAsync(string cpf, int? clienteIdExcluir = null)
    {
        try
        {
            var clienteExistente = await _unitOfWork.ClienteRepository.GetByCpfAsync(cpf);
            
            if (clienteExistente != null && clienteExistente.Id != clienteIdExcluir)
            {
                return OperationResult<bool>.CreateFailure("Já existe um cliente cadastrado com este CPF");
            }

            return OperationResult<bool>.CreateSuccess(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao validar CPF único");
            return OperationResult<bool>.CreateFailure("Erro interno ao validar CPF");
        }
    }
}