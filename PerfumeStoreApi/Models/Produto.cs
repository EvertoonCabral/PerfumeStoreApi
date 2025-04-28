namespace PerfumeStoreApi.Models;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Marca { get; set; }
    public decimal PrecoCompra { get; set; }
    public decimal PrecoVenda { get; set; }
    public int QuantidadeEstoque { get; set; }
}