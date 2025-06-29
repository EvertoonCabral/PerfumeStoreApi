using System.ComponentModel.DataAnnotations;

namespace PerfumeStoreApi.Models;

public class Cliente
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Nome { get; set; }
    [StringLength(11, MinimumLength = 11)]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter 11 dígitos")]
    public string? Cpf { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Telefone { get; set; }
    public bool IsAtivo { get; set; } = true;
    public DateTime DataCadastro { get; set; } = DateTime.Now; // Novo campo
    public virtual ICollection<Venda> Vendas { get; set; }
}