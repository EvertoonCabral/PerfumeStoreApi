using PerfumeStoreApi.Models.Enums;

namespace PerfumeStoreApi.Context.Dtos.Pagamento;

public class CreatePagamentoRequest
{
    public decimal ValorPago { get; set; }
    public TipoFormaPagamento FormaPagamento { get; set; }
    public DateTime? DataPagamento { get; set; } // Se null, usa DateTime.Now
    public string? Observacoes { get; set; }
}