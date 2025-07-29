using PerfumeStoreApi.Context.Dtos;
using PerfumeStoreApi.Context.Dtos.ItemVenda;
using PerfumeStoreApi.Context.Dtos.Pagamento;
using PerfumeStoreApi.Data.Dtos;
using PerfumeStoreApi.Models.Enums;

namespace PerfumeStoreApi.Service.Interfaces;

public interface IVendaService
{
  Task<OperationResult<VendaResponse>> CriarVendaAsync(CreateVendaRequest request);
  Task<OperationResult<VendaResponse>> FinalizarVendaAsync(int vendaId, List<CreatePagamentoRequest> pagamentos);
  Task<OperationResult<VendaResponse>> CancelarVendaAsync(int vendaId, string motivo, string? usuarioResponsavel = null) ;
  Task<OperationResult<VendaResponse>> ObterVendaPorIdAsync(int vendaId);
  Task<List<VendaResponse>> ObterVendasAsync(DateTime? dataInicio = null, DateTime? dataFim = null, 
    StatusVenda? status = null, int? clienteId = null);
  Task ValidarEstoqueParaVendaAsync(List<CreateItemVendaRequest> itens, int estoqueId);

}