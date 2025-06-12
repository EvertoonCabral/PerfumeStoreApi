using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.Context.Dtos;
using AutoMapper;
using PerfumeStoreApi.UnitOfWork;

namespace PerfumeStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ClienteController( IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpGet]
    public async Task <ActionResult<IEnumerable<ClienteDto>>> RetornaClientes()
    {
        var clientes = await _unitOfWork.ClienteRepository.GetAll();

        if (clientes == null || !clientes.Any())
        {
            return NotFound("Nenhum cliente encontrado");
        }
        
        return _mapper.Map<List<ClienteDto>>(clientes);
    }

    [HttpGet("{id}")]
    public async Task <ActionResult<ClienteDto>> RetornaClientePorId(int id)
    {
        var cliente = await _unitOfWork.ClienteRepository.GetById(id);

        if (cliente is null)
        {
            return NotFound("Nenhum cliente encontrado");            
        }
        
        return _mapper.Map<ClienteDto>(cliente);
    }

    [HttpGet("{id}/detalhes")]
    public async Task <ActionResult<ClienteDetalhesDto>> RetornaClienteDetalhes(int id)
    {
        var cliente = await _unitOfWork.ClienteRepository.RetornaClienteDetalhesAsync(id);
            
        if (cliente is null)
        {
            return NotFound("Nenhum cliente encontrado");            
        }
        
        return _mapper.Map<ClienteDetalhesDto>(cliente);
    }

    [HttpPut("{id}")]
    public async  Task<ActionResult<ClienteDto>> AtualizaCliente(int id, ClienteCreateUpdateDto clienteDto)
    {
        var cliente =  await _unitOfWork.ClienteRepository.GetById(id);

        if (cliente is null)
        {
            return NotFound("Nenhum cliente encontrado");
        }
        
        // Mapeia os valores do DTO para a entidade existente
        _mapper.Map(clienteDto, cliente);
        
      await  _unitOfWork.CommitAsync();        
        return _mapper.Map<ClienteDto>(cliente);
    }

    [HttpPost("Criarcliente")]
    public async Task<ActionResult<ClienteCreateUpdateDto>> CriarCliente(ClienteCreateUpdateDto clienteDto)
    {
        var cliente = _mapper.Map<Cliente>(clienteDto);
        
        _unitOfWork.ClienteRepository.Create(cliente);
       await _unitOfWork.CommitAsync();
       
        var novoClienteDto = _mapper.Map<ClienteCreateUpdateDto>(cliente);
        
            return Ok(novoClienteDto);
    }

    [HttpDelete("{id}")]
    public async  Task<ActionResult<ClienteDto>> ExcluirClienteAsync(int id)
    {
        var cliente = await _unitOfWork.ClienteRepository.GetById(id);

        if (cliente is null)
        {
            return NotFound("Nenhum cliente encontrado");
        }
        
        _unitOfWork.ClienteRepository.Delete(cliente);
       await _unitOfWork.CommitAsync();      
        
        return Ok(_mapper.Map<ClienteDto>(cliente));
    }
}