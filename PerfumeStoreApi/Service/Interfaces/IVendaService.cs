using PerfumeStoreApi.Context.Dtos;
using PerfumeStoreApi.Context.Dtos.ItemVenda;
using PerfumeStoreApi.Context.Dtos.Pagamento;
using PerfumeStoreApi.Models.Enums;

namespace PerfumeStoreApi.Service.Interfaces;

public interface IVendaService
{
  Task<VendaResponse> CriarVendaAsync(CreateVendaRequest request);
  Task<VendaResponse> FinalizarVendaAsync(int vendaId, List<CreatePagamentoRequest> pagamentos);
  Task<VendaResponse> CancelarVendaAsync(int vendaId, string motivo, string? usuarioResponsavel = null) ;
  Task<VendaResponse> ObterVendaPorIdAsync(int vendaId);
  Task<List<VendaResponse>> ObterVendasAsync(DateTime? dataInicio = null, DateTime? dataFim = null, 
    StatusVenda? status = null, int? clienteId = null);
  Task ValidarEstoqueParaVendaAsync(List<CreateItemVendaRequest> itens, int estoqueId);

}