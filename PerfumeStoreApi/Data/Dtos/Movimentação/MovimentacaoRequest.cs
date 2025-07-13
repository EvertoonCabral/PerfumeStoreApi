using PerfumeStoreApi.Models.Enums;

namespace PerfumeStoreApi.Data.Dtos.Movimentação;

public class MovimentacaoRequest
{
    public int ProdutoId { get; set; }
    public int EstoqueId { get; set; }
    public int Quantidade { get; set; }
    public TipoMovimentacao Tipo { get; set; }
    public string? Observacoes { get; set; }
    public string? UsuarioResponsavel { get; set; }
}