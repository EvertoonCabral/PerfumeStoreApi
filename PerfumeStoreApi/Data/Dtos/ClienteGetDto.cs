namespace PerfumeStoreApi.Context.Dtos;

public class ClienteGetDto
{
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Telefone { get; set; }
    public bool IsAtivo { get; set; }
}