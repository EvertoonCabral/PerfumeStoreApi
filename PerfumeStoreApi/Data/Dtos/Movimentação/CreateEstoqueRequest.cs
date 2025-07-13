using System.ComponentModel.DataAnnotations;

namespace PerfumeStoreApi.Data.Dtos.Movimentação;

public class CreateEstoqueRequest
{
    [Required(ErrorMessage = "O nome do estoque é obrigatório")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
    public string? Descricao { get; set; }

    [Required(ErrorMessage = "O usuário responsável é obrigatório")]
    [StringLength(100, ErrorMessage = "O usuário responsável deve ter no máximo 100 caracteres")]
    public string UsuarioResponsavel { get; set; } = string.Empty;
}