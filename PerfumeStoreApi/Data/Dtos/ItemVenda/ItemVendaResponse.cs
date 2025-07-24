namespace PerfumeStoreApi.Context.Dtos.ItemVenda;

public class ItemVendaResponse
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public string ProdutoNome { get; set; }
    public string ProdutoMarca { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal => Quantidade * PrecoUnitario;
}