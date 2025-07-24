using PerfumeStoreApi.Context.Dtos.ItemVenda;

namespace PerfumeStoreApi.Context.Dtos;

public class CreateVendaRequest
{
    public int ClienteId { get; set; }
    public int EstoqueId { get; set; } // Estoque de onde sairão os produtos
    public List<CreateItemVendaRequest> Itens { get; set; } = new();
    public decimal Desconto { get; set; } = 0;
    public string? Observacoes { get; set; }
    public string? UsuarioVendedor { get; set; }
}