namespace PerfumeStoreApi.Data.Dtos.Usuario;

public class UsuarioRegisterDto
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public int? ClienteId { get; set; }
}