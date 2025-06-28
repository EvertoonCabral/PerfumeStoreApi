namespace PerfumeStoreApi.Models;

public class Pagamento
{

    public int Id { get; set; }
    public int VendaId { get; set; }
    public virtual Venda Venda { get; set; }

    public DateTime DataPagamento { get; set; }     
    public decimal ValorPago { get; set; }
    public decimal ValorTotal { get; set; }
}