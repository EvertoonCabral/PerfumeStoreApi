using System.ComponentModel.DataAnnotations;

namespace PerfumeStoreApi.Context.Dtos.ProdutoDTO;

public class GetProdutosDto
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Nome { get; set; }
    
    public string Marca { get; set; }
    [Required]
    public decimal PrecoCompra { get; set; }
    [Required]
    public decimal PrecoVenda { get; set; }
    [Required]
    public int QuantidadeEstoque { get; set; }
    [StringLength(500)]
    public string? Descricao { get; set; } 
    public bool IsAtivo { get; set; } = true; 
    public DateTime DataCadastro { get; set; } = DateTime.Now; 
    public int EstoqueId { get; set; } // FK
}