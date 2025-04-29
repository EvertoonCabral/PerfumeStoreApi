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
        var clientes = _context.Clientes.ToListAsync();

        if (clientes == null)
        {
            return NotFound("Nenhum cliente encontrado");
        }
        
        return clientes.Result;
    }
    
    
}