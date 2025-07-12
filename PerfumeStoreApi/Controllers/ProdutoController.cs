using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Context.Dtos.ProdutoDTO;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.Service.Interfaces;
using PerfumeStoreApi.UnitOfWork;

namespace PerfumeStoreApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _produtoService;

    public ProdutoController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<GetProdutosDto>>> GetProdutos()
    {
        var produtos = await _produtoService.ListarProdutosTodosAsync();

        if (produtos is null)
        {
            return NotFound("Nenhum produto encontrado");
        }
        
        return Ok(produtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Produto>> GetProduto(int id)
    {
        var produto = await _produtoService.ObterProdutoAsync(id);

        if (produto is null)
        {
            return NotFound("Nenhum produto encontrado");
        }
        
        return Ok(produto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Produto>> AlterarProduto(int id, Produto produto)
    {
        var produtoAtualizado = await _produtoService.ObterProdutoAsync(id);
        
        if (produtoAtualizado is null)
        {
            return NotFound("O produto não foi encontrado");
        }
        
        // Atualizar usando o service, o commit ocorre no Service tmb
      await _produtoService.AtualizarProdutoAsync(id, produtoAtualizado);
        
        return Ok(produtoAtualizado);
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoDto>> CadastrarProduto(ProdutoCreateUpdateDto produto)
    {
        var novoProduto = await _produtoService.CriarProdutoAsync(produto);
        
        return Ok(novoProduto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Produto>> ExcluirProduto(int id)
    {
        var produto = await _produtoService.ObterProdutoAsync(id);

        if (produto is null)
        {
            return NotFound("Nenhum produto encontrado");
        }
        await _produtoService.ExcluirProdutoAsync(id);
        
        return Ok(produto);
    }
}