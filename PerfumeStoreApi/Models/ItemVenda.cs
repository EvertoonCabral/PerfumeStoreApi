using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PerfumeStoreApi.Models;

public class ItemVenda
{
    
    [Key]
    public int Id { get; set; }
    
    public int VendaId { get; set; }
    public virtual Venda Venda { get; set; }
    
    public int ProdutoId { get; set; }
    public virtual Produto Produto { get; set; }
    
    [Required]
    public int Quantidade { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecoUnitario { get; set; } // CRÍTICO: Preço praticado na venda
    
    // Propriedade calculada
    public decimal Subtotal => Quantidade * PrecoUnitario;
}
