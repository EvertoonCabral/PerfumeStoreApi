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
    public List<ItemVenda> Itens { get; set; } = new();
    public List<Pagamento> Pagamentos { get; set; } = new();
    public int ClienteId { get; set; }
    public virtual Cliente Cliente { get; set; }
}

