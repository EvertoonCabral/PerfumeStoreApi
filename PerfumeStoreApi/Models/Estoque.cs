using System.ComponentModel.DataAnnotations;

namespace PerfumeStoreApi.Models;

public class Estoque
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Nome { get; set; }

    public string? Descricao { get; set; }

    public bool IsAtivo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    
    public virtual ICollection<ItemEstoque> ItensEstoque { get; set; } = new List<ItemEstoque>();
}
