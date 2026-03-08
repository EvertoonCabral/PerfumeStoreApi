using System.ComponentModel.DataAnnotations;
using PerfumeStoreApi.Models.Enums;

namespace PerfumeStoreApi.Models;

public class Usuario
{
    
    [Key] public int Id { get; set; }
    [Required]
    public string Nome { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public byte[] SenhaHash { get; set; }
    [Required]
    public byte[] SenhaSalt { get; set; }
    
    public RoleUsuario Role { get; set; } = RoleUsuario.USER;

    public DateTime DataCadastro { get; set; } = DateTime.Now;

    
    
}