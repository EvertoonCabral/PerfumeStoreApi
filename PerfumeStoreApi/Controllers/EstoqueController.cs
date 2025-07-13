using Microsoft.AspNetCore.Mvc;
using PerfumeStoreApi.Data.Dtos.Movimentação;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.Service.Interfaces;

namespace PerfumeStoreApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstoqueController : ControllerBase
{
    private readonly IEstoqueService _estoqueService;

    public EstoqueController(IEstoqueService estoqueService)
    {
        _estoqueService = estoqueService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CriarEstoque([FromBody] CreateEstoqueRequest request)
    {
        try
        {
            var estoque = await _estoqueService.CriarEstoqueAsync(request);
            return Ok(estoque);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { resonseError = "Erro interno do servidor", ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodosEstoques()
    {
        try
        {
            var estoques = await _estoqueService.ObterTodosEstoquesAsync();
            return Ok(estoques);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Erro interno do servidor" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterEstoquePorId(int id)
    {
        try
        {
            var estoque = await _estoqueService.ObterEstoquePorIdAsync(id);
            if (estoque == null)
                return NotFound(new { Message = "Estoque não encontrado" });
            
            return Ok(estoque);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Erro interno do servidor" });
        }
    }

    [HttpGet("produto/{produtoId}")]
    public async Task<ActionResult<IEnumerable<ItemEstoque>>> ObterEstoqueProduto(int produtoId)
    {
        var itens = await _estoqueService.ObterEstoquePorProdutoAsync(produtoId);
        return Ok(itens);
    }

    [HttpGet("produto/{produtoId}/total")]
    public async Task<ActionResult<int>> ObterQuantidadeTotal(int produtoId)
    {
        var total = await _estoqueService.ObterQuantidadeTotalProdutoAsync(produtoId);
        return Ok(total);
    }

    [HttpGet("estoque/{estoqueId}")]
    public async Task<ActionResult<IEnumerable<ItemEstoque>>> ObterItensEstoque(int estoqueId)
    {
        var itens = await _estoqueService.ObterItensPorEstoqueAsync(estoqueId);
        return Ok(itens);
    }

    [HttpGet("estoque-baixo")]
    public async Task<ActionResult<IEnumerable<ItemEstoque>>> ObterEstoqueBaixo()
    {
        var itens = await _estoqueService.ObterItensComEstoqueBaixoAsync();
        return Ok(itens);
    }

    [HttpPost("movimentar")]
    public async Task<ActionResult> MovimentarEstoque([FromBody] MovimentacaoRequest request)
    {
        try
        {
            var sucesso = await _estoqueService.MovimentarEstoqueAsync(
                request.ProdutoId, 
                request.EstoqueId, 
                request.Quantidade,
                request.Tipo,
                request.Observacoes,
                request.UsuarioResponsavel
            );

            if (sucesso)
                return Ok(new { Message = "Movimentação realizada com sucesso" });
            else
                return BadRequest(new { Message = "Erro ao realizar movimentação" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Erro interno do servidor" });
        }
    }

    [HttpPost("transferir")]
    public async Task<ActionResult> TransferirEstoque([FromBody] TransferenciaRequest request)
    {
        try
        {
            var sucesso = await _estoqueService.TransferirEstoqueAsync(
                request.ProdutoId,
                request.EstoqueOrigemId,
                request.EstoqueDestinoId,
                request.Quantidade,
                request.Observacoes,
                request.UsuarioResponsavel
            );

            if (sucesso)
                return Ok(new { Message = "Transferência realizada com sucesso" });
            else
                return BadRequest(new { Message = "Erro ao realizar transferência" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Erro interno do servidor" });
        }
    }

    [HttpGet("historico/produto/{produtoId}")]
    public async Task<ActionResult<IEnumerable<MovimentacaoEstoque>>> ObterHistorico(
        int produtoId, 
        int? estoqueId = null, 
        DateTime? dataInicio = null, 
        DateTime? dataFim = null)
    {
        var historico = await _estoqueService.ObterHistoricoMovimentacaoAsync(
            produtoId, estoqueId, dataInicio, dataFim);
        return Ok(historico);
    }
}
