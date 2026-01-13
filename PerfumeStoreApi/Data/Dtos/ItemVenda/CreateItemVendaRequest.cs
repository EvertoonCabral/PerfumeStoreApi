namespace PerfumeStoreApi.Data.Dtos.ItemVenda;

public class CreateItemVendaRequest
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal? PrecoUnitario { get; set; } // Se null, usa o preço do produto
}