using PerfumeStoreApi.Context.Dtos;
using PerfumeStoreApi.Context.Dtos.ItemVenda;
using PerfumeStoreApi.Context.Dtos.Pagamento;
using PerfumeStoreApi.Data.Dtos;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.Models.Enums;

namespace PerfumeStoreApi.Service.Interfaces;

public interface IVendaService
{
  Task<OperationResult<VendaResponseDetail>> CriarVendaAsync(CreateVendaRequest request);
  Task<OperationResult<VendaResponseDetail>> FinalizarVendaAsync(int vendaId, List<CreatePagamentoRequest> pagamentos);
  Task<OperationResult<VendaResponseDetail>> CancelarVendaAsync(int vendaId, string motivo, string? usuarioResponsavel = null) ;
  Task<OperationResult<VendaResponseDetail>> ObterVendaPorIdAsync(int vendaId);
  Task<List<VendaResponse>> ObterVendasAsync(DateTime? dataInicio = null, DateTime? dataFim = null, 
    StatusVenda? status = null, int? clienteId = null);
  Task ValidarEstoqueParaVendaAsync(List<CreateItemVendaRequest> itens, int estoqueId);

}