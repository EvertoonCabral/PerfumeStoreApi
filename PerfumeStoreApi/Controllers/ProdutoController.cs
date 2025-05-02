using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutoController : ControllerBase
{

    private readonly AppDbContext _context;
    
    public ProdutoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<Produto>>> GetProdutos()
    {
        var produtos = await _context.Produtos.ToListAsync();

        if (produtos is null)
        {
            return NotFound("Nenhum produto encontrado");
        }
        
        return Ok(produtos);
        
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Produto>> GetProduto(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);

        if (produto is null)
        {
            return NotFound("Nenhum produto encontrado");
        }
        
        return Ok(produto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Produto>> AlterarProduto(int id, Produto produto)
    {
        
        var produtoAtualizado = await _context.Produtos.FindAsync(id);
        
        if (produtoAtualizado is null)
        {
            return NotFound("O produto não foi encontrado");
        }
        
        _context.Entry(produtoAtualizado).CurrentValues.SetValues(produto);
        await _context.SaveChangesAsync();
        
        return Ok(produtoAtualizado);
    }

    [HttpPost]
    public async Task<ActionResult<Produto>> CadastrarProduto(Produto produto)
    {

        var novoProduto = await _context.Produtos.AddAsync(produto);
         _context.SaveChanges();
        
        return Ok(novoProduto.Entity);
        
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Produto>> ExcluirProduto(int id)
    {
        
        var produto = await _context.Produtos.FindAsync(id);

        if (produto is null)
        {
            return NotFound("Nenhum produto encontrado");
        }
         
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
        
        return Ok(produto);
        
    }
    
    
    
}
