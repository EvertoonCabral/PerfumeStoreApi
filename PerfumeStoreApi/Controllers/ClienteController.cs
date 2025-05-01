using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    
    private readonly AppDbContext _context;

    public ClienteController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> RetornaClientes()
    {
        var clientes =  await _context.Clientes.ToListAsync();

        if (clientes == null)
        {
            return  NotFound("Nenhum cliente encontrado");
        }
        
        return clientes;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> RetornaClientePorId(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente is null)
        {
            return NotFound("Nenhum cliente encontrado");            
        }
        return cliente;

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Cliente>> AtualizaCliente(int id, Cliente cliente)
    {
        
        var cli = await _context.Clientes.FindAsync(id);

        if (cli is null)
        {
            return NotFound("Nenhum cliente encontrado");
        }
        
        _context.Entry(cliente).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        
        return cliente;
        
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> CriarCliente(Cliente cliente)
    {
        _context.Clientes.Add(cliente);

         await _context.SaveChangesAsync();
         
         
         return CreatedAtAction(nameof(RetornaClientePorId), new { id = cliente.Id }, cliente);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Cliente>> ExcluirCliente(int id)
    {
        
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente is null)
        {
            return NotFound("Nenhum cliente encontrado");
        }
        
        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();
        
        return Ok(cliente);
    }
    
    
    
}