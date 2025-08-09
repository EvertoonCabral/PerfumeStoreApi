using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.Service.Interfaces;
using PerfumeStoreApi.UnitOfWork;

namespace PerfumeStoreApi.Service;

public class AuthService : IAuthService
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;


    public AuthService(IUnitOfWork unitOfWork, IConfiguration config)
    {
        _unitOfWork = unitOfWork;
        _config = config;
    }


    public async Task<string> RegistrarAsync(Usuario usuario, string senha)
    {
        CriarSenhaHash(senha, out byte[] hash, out byte[] salt);
        usuario.SenhaHash = hash;
        usuario.SenhaSalt = salt;

        _unitOfWork.UsuarioRepository.Create(usuario);
        await _unitOfWork.CommitAsync();

        return GerarToken(usuario);
    }

    public async Task<string?> LoginAsync(string email, string senha)
    {
        var usuario = await _unitOfWork.UsuarioRepository.GetByEmailAsync(email);
        if (usuario == null || !VerificarSenhaHash(senha, usuario.SenhaHash, usuario.SenhaSalt))
            return null;

        return GerarToken(usuario);
    }
    
    
    private void CriarSenhaHash(string senha, out byte[] hash, out byte[] salt)
    {
        using var hmac = new HMACSHA512();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
    }
    
    private bool VerificarSenhaHash(string senha, byte[] hashArmazenado, byte[] salt)
    {
        using var hmac = new HMACSHA512(salt);
        var hashComputado = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
        return hashComputado.SequenceEqual(hashArmazenado);
    }
    
    private string GerarToken(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Role, usuario.Role.ToString())
        };

        if (usuario.ClienteId.HasValue)
            claims.Add(new Claim("ClienteId", usuario.ClienteId.Value.ToString()));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}