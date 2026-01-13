using System.ComponentModel.DataAnnotations;
using PerfumeStoreApi.Context.Dtos.ItemVenda;
using PerfumeStoreApi.Data.Dtos.ItemVenda;

namespace PerfumeStoreApi.Context.Dtos;

public class ValidarEstoqueRequest
{
    [Required]
    public int EstoqueId { get; set; }

    [Required]
    public List<CreateItemVendaRequest> Itens { get; set; } = new();
}