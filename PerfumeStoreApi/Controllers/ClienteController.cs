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
    public ActionResult<IEnumerable<ClienteDto>> RetornaClientes()
    {
        var clientes =  _unitOfWork.ClienteRepository.GetAll();

        if (clientes == null || !clientes.Any())
        {
            return NotFound("Nenhum cliente encontrado");
        }
        
        return _mapper.Map<List<ClienteDto>>(clientes);
    }

    [HttpGet("{id}")]
    public ActionResult<ClienteDto> RetornaClientePorId(int id)
    {
        var cliente =  _unitOfWork.ClienteRepository.GetById(id);

        if (cliente is null)
        {
            return NotFound("Nenhum cliente encontrado");            
        }
        
        return _mapper.Map<ClienteDto>(cliente);
    }

    [HttpGet("{id}/detalhes")]
    public ActionResult<ClienteDetalhesDto> RetornaClienteDetalhes(int id)
    {
        var cliente = _unitOfWork.ClienteRepository.RetornaClienteDetalhes(id);
            
        if (cliente is null)
        {
            return NotFound("Nenhum cliente encontrado");            
        }
        
        return _mapper.Map<ClienteDetalhesDto>(cliente);
    }

    [HttpPut("{id}")]
    public ActionResult<ClienteDto> AtualizaCliente(int id, ClienteCreateUpdateDto clienteDto)
    {
        var cliente =  _unitOfWork.ClienteRepository.GetById(id);

        if (cliente is null)
        {
            return NotFound("Nenhum cliente encontrado");
        }
        
        // Mapeia os valores do DTO para a entidade existente
        _mapper.Map(clienteDto, cliente);
        
        _unitOfWork.Commit();        
        return _mapper.Map<ClienteDto>(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteDto>> CriarCliente(ClienteCreateUpdateDto clienteDto)
    {
        var cliente = _mapper.Map<Cliente>(clienteDto);
        
        _unitOfWork.ClienteRepository.Create(cliente);
        _unitOfWork.Commit();
        
        var novoClienteDto = _mapper.Map<ClienteDto>(cliente);
        
        return CreatedAtAction(nameof(RetornaClientePorId), new { id = cliente.Id }, novoClienteDto);
    }

    [HttpDelete("{id}")]
    public ActionResult<ClienteDto> ExcluirCliente(int id)
    {
        var cliente = _unitOfWork.ClienteRepository.GetById(id);

        if (cliente is null)
        {
            return NotFound("Nenhum cliente encontrado");
        }
        
        _unitOfWork.ClienteRepository.Delete(cliente);
        _unitOfWork.Commit();      
        
        return Ok(_mapper.Map<ClienteDto>(cliente));
    }
}