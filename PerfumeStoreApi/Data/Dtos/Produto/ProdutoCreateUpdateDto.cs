using System.ComponentModel.DataAnnotations;

namespace PerfumeStoreApi.Data.Dtos.Produto;

public class ProdutoCreateUpdateDto
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
    [StringLength(500)]
    public string? Descricao { get; set; } 
    
    public bool IsAtivo { get; set; } = true; 
}