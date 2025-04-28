namespace PerfumeStoreApi.Models;

public class ItemVenda
{
    public int Id { get; set; }
    public int VendaId  { get; set; }
    public int  ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    
    
}