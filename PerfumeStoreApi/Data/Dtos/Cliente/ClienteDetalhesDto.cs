using PerfumeStoreApi.Context.Dtos;

namespace PerfumeStoreApi.Data.Dtos.Cliente;

public class ClienteDetalhesDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }
    public string? Email { get; set; }
    public DateTime DataCadastro { get; set; }
    public string Telefone { get; set; }
    public bool IsAtivo { get; set; }
    public ICollection<VendaResumoDto> Vendas { get; set; } = new List<VendaResumoDto>();
}