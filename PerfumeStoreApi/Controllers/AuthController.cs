using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PerfumeStoreApi.Data.Dtos.Usuario;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.Service.Interfaces;
using PerfumeStoreApi.UnitOfWork;

namespace PerfumeStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(UsuarioRegisterDto dto)
    {
        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            ClienteId = dto.ClienteId 
        };

        var token = await _authService.RegistrarAsync(usuario, dto.Senha);
        return Ok(new { token });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(UsuarioLoginDto dto)
    {
        var token = await _authService.LoginAsync(dto.Email, dto.Senha);
        if (token == null)
            return Unauthorized("Credenciais inválidas");

        return Ok(new { token });
    }
    
}