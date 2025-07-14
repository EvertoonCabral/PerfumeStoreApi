
namespace PerfumeStoreApi.Data.Dtos.ItemVenda;

public class ItemEstoqueResponse
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public int EstoqueId { get; set; }
    public int Quantidade { get; set; }
    public int? QuantidadeMinima { get; set; } 
    public int? QuantidadeMaxima { get; set; } 
    public DateTime DataUltimaMovimentacao { get; set; } = DateTime.Now;
}