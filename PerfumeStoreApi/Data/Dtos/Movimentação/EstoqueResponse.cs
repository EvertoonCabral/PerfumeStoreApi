namespace PerfumeStoreApi.Data.Dtos.Movimentação;

public class EstoqueResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime DataCriacao { get; set; }
    public int TotalItens { get; set; }
    public int TotalProdutos { get; set; }
}