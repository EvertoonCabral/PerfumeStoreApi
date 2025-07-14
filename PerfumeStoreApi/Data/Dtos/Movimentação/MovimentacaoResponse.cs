using PerfumeStoreApi.Models.Enums;

namespace PerfumeStoreApi.Data.Dtos.Movimentação;

public class MovimentacaoResponse
{
    public int Id { get; set; }
    public int? ItemEstoqueId { get; set; }
    public int Quantidade { get; set; }
    public TipoMovimentacao Tipo { get; set; }
    public string? Observacoes { get; set; }
    public string? UsuarioResponsavel { get; set; }
}