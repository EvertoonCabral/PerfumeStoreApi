using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PerfumeStoreApi.Models.Enums;

namespace PerfumeStoreApi.Models;

public class Pagamento
{

    [Key]
    public int Id { get; set; }
    
    public int VendaId { get; set; }
    public virtual Venda Venda { get; set; }
    
    [Required]
    public DateTime DataPagamento { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal ValorPago { get; set; }
    
    [Required]
    public TipoFormaPagamento FormaPagamento { get; set; }
    
    public DateTime? DataVencimento { get; set; } // Para crediário (30 dias)
    public string? Observacoes { get; set; }
}