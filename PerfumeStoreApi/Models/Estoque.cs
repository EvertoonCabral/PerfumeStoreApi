using System.ComponentModel.DataAnnotations;

namespace PerfumeStoreApi.Models;

public class Estoque
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Nome { get; set; }

    public string? Descricao { get; set; }

    // Propriedade de navegação
    public virtual ICollection<Produto> ProdutosEstoque { get; set; } = new List<Produto>();

    public DateTime DataCriacao { get; set; } = DateTime.Now;
}
