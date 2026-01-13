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
    private readonly IUnitOfWork _unitOfWork;

    public AuthController(IAuthService authService, IUnitOfWork unitOfWork)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(UsuarioRegisterDto dto)
    {
        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
        };

        var result = await _authService.RegistrarAsync(usuario, dto.Senha);
        
        if (!result.Success)
            return BadRequest(result);
        
        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(UsuarioLoginDto dto)
    {
        var result = await _authService.LoginAsync(dto.Email, dto.Senha);

        if (!result.Success)
            return Unauthorized(result);

        return Ok(result);
    }
    
}