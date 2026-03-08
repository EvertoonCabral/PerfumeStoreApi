using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> Register([FromBody] UsuarioRegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
        };

        var result = await _authService.RegistrarAsync(usuario, dto.Senha);
        
        if (!result.Success)
            return BadRequest(result);
        
        return StatusCode(StatusCodes.Status201Created, result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UsuarioLoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(dto.Email, dto.Senha);

        if (!result.Success)
            return Unauthorized(result);

        return Ok(result);
    }

    /// <summary>
    /// Invalida o token JWT atual do usuário autenticado.
    /// </summary>
    /// <returns>Confirmação de logout.</returns>
    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        var jti = User.FindFirstValue(JwtRegisteredClaimNames.Jti);
        var expClaim = User.FindFirstValue(JwtRegisteredClaimNames.Exp);

        if (string.IsNullOrEmpty(jti))
            return BadRequest(new { message = "Token inválido." });

        DateTimeOffset expiry;
        if (long.TryParse(expClaim, out var expUnix))
            expiry = DateTimeOffset.FromUnixTimeSeconds(expUnix);
        else
            expiry = DateTimeOffset.UtcNow.AddHours(2);

        _authService.Logout(jti, expiry);

        return Ok(new { message = "Logout realizado com sucesso." });
    }
}