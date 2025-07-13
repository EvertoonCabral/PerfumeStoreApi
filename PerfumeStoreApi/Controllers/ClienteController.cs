using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.Context.Dtos;
using AutoMapper;
using PerfumeStoreApi.Data.Dtos.Cliente;
using PerfumeStoreApi.Service.Interfaces;
using PerfumeStoreApi.UnitOfWork;

namespace PerfumeStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClienteController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    /// <summary>
    /// Retorna todos os clientes com paginação e filtros
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResult<ClienteDto>>> GetClientes([FromQuery] ClienteFiltroDto filtros)
    {
        var resultado = await _clienteService.GetClientesAsync(filtros);
        
        if (!resultado.Success)
        {
            return BadRequest(new { errors = resultado.Errors });
        }

        return Ok(resultado.Data);
    }

    /// <summary>
    /// Retorna um cliente específico por ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClienteDto>> GetClienteById(int id)
    {
        var resultado = await _clienteService.GetClienteByIdAsync(id);
        
        if (!resultado.Success)
        {
            return resultado.Errors.Any(e => e.Contains("não encontrado")) 
                ? NotFound(new { message = resultado.Errors.First() })
                : BadRequest(new { errors = resultado.Errors });
        }

        return Ok(resultado.Data);
    }

    /// <summary>
    /// Retorna detalhes completos do cliente incluindo histórico de vendas
    /// </summary>
    [HttpGet("{id:int}/detalhes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteDetalhesDto>> GetClienteDetalhes(int id)
    {
        var resultado = await _clienteService.GetClienteDetalhesAsync(id);
        
        if (!resultado.Success)
        {
            return resultado.Errors.Any(e => e.Contains("não encontrado"))
                ? NotFound(new { message = resultado.Errors.First() })
                : BadRequest(new { errors = resultado.Errors });
        }

        return Ok(resultado.Data);
    }

    /// <summary>
    /// Cria um novo cliente
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ClienteDto>> CreateCliente([FromBody] ClienteCreateUpdateDto clienteDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var resultado = await _clienteService.CreateClienteAsync(clienteDto);
        
        if (!resultado.Success)
        {
            return resultado.Errors.Any(e => e.Contains("CPF"))
                ? Conflict(new { errors = resultado.Errors })
                : BadRequest(new { errors = resultado.Errors });
        }

        return CreatedAtAction(
            nameof(GetClienteById),
            new { id = resultado.Data!.Id },
            resultado.Data);
    }

    /// <summary>
    /// Atualiza um cliente existente
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ClienteDto>> UpdateCliente(int id, [FromBody] ClienteCreateUpdateDto clienteDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var resultado = await _clienteService.UpdateClienteAsync(id, clienteDto);
        
        if (!resultado.Success)
        {
            if (resultado.Errors.Any(e => e.Contains("não encontrado")))
                return NotFound(new { message = resultado.Errors.First() });
            
            if (resultado.Errors.Any(e => e.Contains("CPF")))
                return Conflict(new { errors = resultado.Errors });
                
            return BadRequest(new { errors = resultado.Errors });
        }

        return Ok(resultado.Data);
    }

    /// <summary>
    /// Desativa um cliente (soft delete)
    /// </summary>
    [HttpPatch("{id:int}/desativar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DesativarCliente(int id)
    {
        var resultado = await _clienteService.DesativarClienteAsync(id);
        
        if (!resultado.Success)
        {
            return resultado.Errors.Any(e => e.Contains("não encontrado"))
                ? NotFound(new { message = resultado.Errors.First() })
                : BadRequest(new { errors = resultado.Errors });
        }

        return Ok(new { message = resultado.Message });
    }

    /// <summary>
    /// Remove um cliente permanentemente (apenas se não tiver vendas)
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteCliente(int id)
    {
        var resultado = await _clienteService.DeleteClienteAsync(id);
        
        if (!resultado.Success)
        {
            return resultado.Errors.Any(e => e.Contains("não encontrado"))
                ? NotFound(new { message = resultado.Errors.First() })
                : BadRequest(new { errors = resultado.Errors });
        }

        return Ok(new { message = resultado.Message });
    }
}