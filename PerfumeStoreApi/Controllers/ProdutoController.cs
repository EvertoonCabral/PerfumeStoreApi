using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.UnitOfWork;

namespace PerfumeStoreApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutoController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ProdutoController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<Produto>>> GetProdutos()
    {
        var produtos = await _unitOfWork.ProdutoRepository.GetAll();

        if (produtos is null)
        {
            return NotFound("Nenhum produto encontrado");
        }
        
        return Ok(produtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Produto>> GetProduto(int id)
    {
        var produto = await _unitOfWork.ProdutoRepository.GetById(id);

        if (produto is null)
        {
            return NotFound("Nenhum produto encontrado");
        }
        
        return Ok(produto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Produto>> AlterarProduto(int id, Produto produto)
    {
        var produtoAtualizado = await _unitOfWork.ProdutoRepository.GetById(id);
        
        if (produtoAtualizado is null)
        {
            return NotFound("O produto não foi encontrado");
        }
        
        // Atualizar usando o repository
        _unitOfWork.ProdutoRepository.Update(produtoAtualizado, produto);
        await _unitOfWork.CommitAsync();
        
        return Ok(produtoAtualizado);
    }

    [HttpPost]
    public async Task<ActionResult<Produto>> CadastrarProduto(Produto produto)
    {
        var novoProduto = _unitOfWork.ProdutoRepository.Create(produto);
        await _unitOfWork.CommitAsync();
        
        return Ok(novoProduto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Produto>> ExcluirProduto(int id)
    {
        var produto = await _unitOfWork.ProdutoRepository.GetById(id);

        if (produto is null)
        {
            return NotFound("Nenhum produto encontrado");
        }
         
        _unitOfWork.ProdutoRepository.Delete(produto);
        await _unitOfWork.CommitAsync();
        
        return Ok(produto);
    }
}