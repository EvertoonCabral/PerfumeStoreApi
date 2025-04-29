using System.ComponentModel.DataAnnotations;

namespace PerfumeStoreApi.Models;

public class Cliente
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Nome { get; set; }
    
    public string? Cpf { get; set; }
    
    [Required]
    public string Telefone { get; set; }
    
    public bool IsAtivo { get; set; } = false;

    public virtual ICollection<Venda> Vendas { get; set; }
}