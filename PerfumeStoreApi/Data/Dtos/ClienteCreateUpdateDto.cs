namespace PerfumeStoreApi.Context.Dtos;

public class ClienteCreateUpdateDto
{
    
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string Telefone { get; set; }
    public bool IsAtivo { get; set; }

}