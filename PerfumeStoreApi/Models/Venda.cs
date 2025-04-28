namespace PerfumeStoreApi.Models;

public class Venda
{

    public int Id { get; set; }
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    public DateTime DataVenda { get; set; }
    public decimal ValorTotal { get; set; }

    public List<ItemVenda> Itens { get; set; } = new();
    public List<Pagamento> Pagamentos { get; set; } = new();
    
}

