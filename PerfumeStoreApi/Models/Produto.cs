using System.ComponentModel.DataAnnotations;

namespace PerfumeStoreApi.Models;

public class Produto
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Nome { get; set; }
    
    public string Marca { get; set; }
    [Required]
    public decimal PrecoCompra { get; set; }
    public decimal PrecoVenda { get; set; }
    [StringLength(500)]
    public string? Descricao { get; set; } 
    public bool IsAtivo { get; set; } = true; 
    
    public DateTime DataCadastro { get; set; } = DateTime.Now; 
    public virtual ICollection<ItemEstoque> ItensEstoque { get; set; } = new List<ItemEstoque>();

}