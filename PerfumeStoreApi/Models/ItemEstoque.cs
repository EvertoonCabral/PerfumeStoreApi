using System.ComponentModel.DataAnnotations;

namespace PerfumeStoreApi.Models;

public class ItemEstoque
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int ProdutoId { get; set; }
    public virtual Produto Produto { get; set; }
    
    [Required]
    public int EstoqueId { get; set; }
    public virtual Estoque Estoque { get; set; }
    
    [Required]
    public int Quantidade { get; set; }
    public int? QuantidadeMinima { get; set; } // Para alertas
    public int? QuantidadeMaxima { get; set; } // Para controle
    
    public DateTime DataUltimaMovimentacao { get; set; } = DateTime.Now;
    
    // Navegação para movimentações
    public virtual ICollection<MovimentacaoEstoque> Movimentacoes { get; set; } = new List<MovimentacaoEstoque>();   
}

