using System.ComponentModel.DataAnnotations;

namespace PerfumeStoreApi.Data.Dtos.Produto;

public class GetProdutosDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Marca { get; set; }
    public decimal PrecoCompra { get; set; }
    public decimal PrecoVenda { get; set; }
    public string? Descricao { get; set; } 
    public bool IsAtivo { get; set; } = true; 
    public DateTime DataCadastro { get; set; } = DateTime.Now; 
    public int EstoqueId { get; set; } 
}