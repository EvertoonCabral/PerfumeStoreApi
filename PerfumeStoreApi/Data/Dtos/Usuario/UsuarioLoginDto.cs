using System.ComponentModel.DataAnnotations;

namespace PerfumeStoreApi.Data.Dtos.Usuario;

public class UsuarioLoginDto
{
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Senha é obrigatória")]
    public string Senha { get; set; }
}