namespace PerfumeStoreApi.Models;

public class ItemVenda
{
    public int Id { get; set; }
    public int VendaId { get; set; }
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }

    public virtual Venda Venda { get; set; } 
    public virtual Produto Produto { get; set; }  
}
