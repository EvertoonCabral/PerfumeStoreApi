namespace PerfumeStoreApi.Data.Dtos.Movimentação;
public class TransferenciaRequest
{
    public int ProdutoId { get; set; }
    public int EstoqueOrigemId { get; set; }
    public int EstoqueDestinoId { get; set; }
    public int Quantidade { get; set; }
    public string? Observacoes { get; set; }
    public string? UsuarioResponsavel { get; set; }
}