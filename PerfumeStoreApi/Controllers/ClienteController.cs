using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.Context.Dtos;
using AutoMapper;

namespace PerfumeStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ClienteController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> RetornaClientes()
    {
        var clientes = await _context.Clientes.ToListAsync();

        if (clientes == null || !clientes.Any())
        {
            return NotFound("Nenhum cliente encontrado");
        }
        
        return _mapper.Map<List<ClienteDto>>(clientes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDto>> RetornaClientePorId(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente is null)
        {
            return NotFound("Nenhum cliente encontrado");            
        }
        
        return _mapper.Map<ClienteDto>(cliente);
    }

    [HttpGet("{id}/detalhes")]
    public async Task<ActionResult<ClienteDetalhesDto>> RetornaClienteDetalhes(int id)
    {
        var cliente = await _context.Clientes
            .Include(c => c.Vendas)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente is null)
        {
            return NotFound("Nenhum cliente encontrado");            
        }
        
        return _mapper.Map<ClienteDetalhesDto>(cliente);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClienteDto>> AtualizaCliente(int id, ClienteCreateUpdateDto clienteDto)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente is null)
        {
            return NotFound("Nenhum cliente encontrado");
        }
        
        // Mapeia os valores do DTO para a entidade existente
        _mapper.Map(clienteDto, cliente);
        
        await _context.SaveChangesAsync();
        
        return _mapper.Map<ClienteDto>(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteDto>> CriarCliente(ClienteCreateUpdateDto clienteDto)
    {
        var cliente = _mapper.Map<Cliente>(clienteDto);
        
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        
        var novoClienteDto = _mapper.Map<ClienteDto>(cliente);
        
        return CreatedAtAction(nameof(RetornaClientePorId), new { id = cliente.Id }, novoClienteDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ClienteDto>> ExcluirCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente is null)
        {
            return NotFound("Nenhum cliente encontrado");
        }
        
        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();
        
        return Ok(_mapper.Map<ClienteDto>(cliente));
    }
}