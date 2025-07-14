using PerfumeStoreApi.Models.Enums;
using System.Text.Json.Serialization;


namespace PerfumeStoreApi.Data.Dtos.Movimentação;

public class MovimentacaoResponse
{
    public int Id { get; set; }
    public int? ItemEstoqueId { get; set; }
    public int Quantidade { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TipoMovimentacao Tipo { get; set; }
    public string? Observacoes { get; set; }
    public string? UsuarioResponsavel { get; set; }
}