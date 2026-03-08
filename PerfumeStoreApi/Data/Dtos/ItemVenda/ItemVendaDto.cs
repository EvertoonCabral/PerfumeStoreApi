namespace PerfumeStoreApi.Data.Dtos.ItemVenda;

public class ItemVendaDto
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }  
}