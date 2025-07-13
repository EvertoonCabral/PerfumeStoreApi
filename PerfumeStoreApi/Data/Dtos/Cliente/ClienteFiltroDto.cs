namespace PerfumeStoreApi.Data.Dtos.Cliente;

public class ClienteFiltroDto
{
    public string? Nome { get; set; }
    public string? Cpf { get; set; }
    public string? Email { get; set; }
    public bool? IsAtivo { get; set; }
    public DateTime? DataCadastroInicio { get; set; }
    public DateTime? DataCadastroFim { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}