using PerfumeStoreApi.Models.Enums;

namespace PerfumeStoreApi.Context.Dtos.Pagamento;

public class PagamentoResponse
{
    public int Id { get; set; }
    public DateTime DataPagamento { get; set; }
    public decimal ValorPago { get; set; }
    public TipoFormaPagamento FormaPagamento { get; set; }
    public DateTime? DataVencimento { get; set; }
    public string? Observacoes { get; set; }
    public string? NumeroTransacao { get; set; }
    public string FormaPagamentoDescricao => FormaPagamento switch
    {
        TipoFormaPagamento.Dinheiro => "Dinheiro",
        TipoFormaPagamento.CartaoCredito => "Cartão de Crédito",
        TipoFormaPagamento.CartaoDebito => "Cartão de Débito",
        TipoFormaPagamento.PIX => "PIX",
        TipoFormaPagamento.Crediario => "Crediário (30 dias)",
        _ => "Desconhecido"
    };
    public bool EstaVencido => DataVencimento.HasValue && DataVencimento.Value < DateTime.Now;
}