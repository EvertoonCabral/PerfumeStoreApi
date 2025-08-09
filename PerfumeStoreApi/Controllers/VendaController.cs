using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PerfumeStoreApi.Context.Dtos;
using PerfumeStoreApi.Context.Dtos.ItemVenda;
using PerfumeStoreApi.Context.Dtos.Pagamento;
using PerfumeStoreApi.Data.Dtos;
using PerfumeStoreApi.Service;
using PerfumeStoreApi.Service.Interfaces;

namespace PerfumeStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VendaController : ControllerBase
{
    private readonly IVendaService _vendaService;

    public VendaController(IVendaService vendaService)
    {
        _vendaService = vendaService;
    }

    [HttpPost("/CadastrarVenda")]
    public async Task<ActionResult<VendaResponse>> CadastrarVendaAsync([FromBody] CreateVendaRequest request)
    {
        //validar o metodo de criar venda, esta com mais de um commit, gerando possibilidade de alterar os dados em um momento do metodo e depois estourar uma expection e so meio processo ser salvo
        var resultado = await _vendaService.CriarVendaAsync(request);

        if (!resultado.Success)
        {
            return BadRequest(resultado.Errors);
        }

        return Ok(resultado.Data);
    }

    [HttpGet("/ObterVendaPorId/{id}")]
    public async Task<ActionResult<VendaResponse>> ObterVendaPorIdAsync(int id)
    {
        var resultado = await _vendaService.ObterVendaPorIdAsync(id);

        if (!resultado.Success)
            return NotFound(resultado.Errors);

        return Ok(resultado.Data);
    }

    [HttpGet("/ObterVendas")]
    public async Task<ActionResult<List<VendaResponse>>> ObterVendasAsync()
    {
        var resultado = await _vendaService.ObterVendasAsync();
        if (resultado == null)
        {
            return NotFound("Nenhum venda foi encontrado.");
        }

        return Ok(resultado);
    }

    [HttpPut("/CancelarVenda")]
    public async Task<ActionResult<VendaResponse>> CancelarVendaAsync(int id, string motivo, string? usuarioResponsavel)
    {
        var result = await _vendaService.CancelarVendaAsync(id, motivo, "ADMIN");

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result);
    }

    /// <summary>
    /// Finaliza uma venda pendente, registrando os pagamentos.
    /// </summary>
    /// <param name="vendaId">ID da venda a ser finalizada.</param>
    /// <param name="pagamentos">
    /// Formas de pagamento disponíveis:
    /// <list type="bullet">
    /// <item>
    /// <description>1 - Dinheiro</description>
    /// </item>
    /// <item>
    /// <description>2 - Cartão de Crédito</description>
    /// </item>
    /// <item>
    /// <description>3 - Cartão de Débito</description>
    /// </item>
    /// <item>
    /// <description>4 - PIX</description>
    /// </item>
    /// <item>
    /// <description>5 - Crediário (pagamento em 30 dias)</description>
    /// </item>
    /// </list>
    /// </param>
    /// <returns>Retorna o resultado da operação com os dados da venda finalizada.</returns>
    [HttpPut("/FinalizarVenda")]
    public async Task<ActionResult<OperationResult<VendaResponse>>> FinalizarVendaAsync(
        int vendaId,
        List<CreatePagamentoRequest> pagamentos)
    {
        var result = await _vendaService.FinalizarVendaAsync(vendaId, pagamentos);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result);
    }


    [HttpPost("/ValidarEstoque")]
    public async Task<IActionResult> ValidarEstoqueAsync([FromBody] ValidarEstoqueRequest request)
    {
        try
        {
            await _vendaService.ValidarEstoqueParaVendaAsync(request.Itens, request.EstoqueId);
            return Ok(new { mensagem = "Estoque validado com sucesso. Todos os produtos estão disponíveis." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { erro = "Erro interno ao validar estoque." });
        }
    }
}
