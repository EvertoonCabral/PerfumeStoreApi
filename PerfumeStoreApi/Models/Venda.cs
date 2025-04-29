using System.ComponentModel.DataAnnotations;

namespace PerfumeStoreApi.Models;

public class Venda
{

    [Key]
    public int Id { get; set; }
    [Required]
    public DateTime DataVenda { get; set; }
    [Required]
    public decimal ValorTotal { get; set; }
    
    public virtual ICollection<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
    public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();

    
    public int ClienteId { get; set; }
    public virtual Cliente Cliente { get; set; }
}

