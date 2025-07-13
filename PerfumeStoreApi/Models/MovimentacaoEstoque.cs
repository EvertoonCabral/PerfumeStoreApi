using System.ComponentModel.DataAnnotations;
using PerfumeStoreApi.Models.Enums;

namespace PerfumeStoreApi.Models;

public class MovimentacaoEstoque
{
    
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int ItemEstoqueId { get; set; }
    public virtual ItemEstoque ItemEstoque { get; set; }
    
    [Required]
    public TipoMovimentacao Tipo { get; set; }
    
    [Required]
    public int Quantidade { get; set; }
    public int QuantidadeAnterior { get; set; }
    public int QuantidadePosterior { get; set; }
    
    public string? Observacoes { get; set; }
    
    public DateTime DataMovimentacao { get; set; } = DateTime.Now;
    public string? UsuarioResponsavel { get; set; }
    
}