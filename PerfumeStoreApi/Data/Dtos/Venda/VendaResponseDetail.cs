using PerfumeStoreApi.Models.Enums;

namespace PerfumeStoreApi.Context.Dtos;

public class VendaResponseDetail
{
    public int Id { get; set; }
    public int ClienteId {get; set;}
    public DateTime DataVenda { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal Desconto { get; set; }
    public StatusVenda Status { get; set; }
    public string? Observacoes { get; set; }
    public string? UsuarioVendedor { get; set; }
}