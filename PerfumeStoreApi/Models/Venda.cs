using System.ComponentModel.DataAnnotations;
using PerfumeStoreApi.Models.Enums;

namespace PerfumeStoreApi.Models;

public class Venda
{

    [Key]
    public int Id { get; set; }
    [Required]
    public DateTime DataVenda { get; set; }
    [Required]
    public decimal ValorTotal { get; set; }
    
    public decimal Desconto { get; set; } = 0;
    public StatusVenda Status { get; set; } = StatusVenda.Pendente;
    public string? Observacoes { get; set; }
    public string? UsuarioVendedor { get; set; }
    
    public virtual ICollection<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
    public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();

    
    public int ClienteId { get; set; }
    public virtual Cliente Cliente { get; set; }
    
    public decimal ValorBruto => Itens?.Sum(i => i.Subtotal) ?? 0;
    public decimal ValorPago => Pagamentos?.Sum(p => p.ValorPago) ?? 0;
    public decimal Saldo => ValorTotal - ValorPago;
    public bool EstaPaga => Saldo <= 0;
}

