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
    private readonly IMapper _mapper;

    public ProdutoController(IProdutoService produtoService, IMapper mapper)
    {
        _produtoService = produtoService;
        _mapper = mapper;
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
    public async Task<ActionResult<GetProdutosDto>> GetProduto(int id)
    {
        var produto = await _produtoService.ObterProdutoAsync(id);

        if (produto is null)
        {
            return NotFound("Nenhum produto encontrado");
        }
        
        return Ok(produto);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> AlterarProduto(int id, ProdutoCreateUpdateDto produtoDto)
    {
        var atualizado = await _produtoService.AtualizarProdutoAsync(id, produtoDto);

        if (atualizado == null)
            return NotFound();

        return Ok(atualizado);
    }


    [HttpPost]
    public async Task<ActionResult<ProdutoDto>> CadastrarProduto(ProdutoCreateUpdateDto produto)
    {
        var novoProduto = await _produtoService.CriarProdutoAsync(produto);
        
        return Ok(novoProduto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ProdutoDto>> ExcluirProduto(int id)
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