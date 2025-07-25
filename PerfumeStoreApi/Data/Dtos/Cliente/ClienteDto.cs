namespace PerfumeStoreApi.Data.Dtos.Cliente;

public class ClienteDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string? Cpf { get; set; }
    public string Telefone { get; set; }
    public string? Email { get; set; }
    public bool IsAtivo { get; set; }
    public DateTime DataCadastro { get; set; }
}