using PerfumeStoreApi.Context.Dtos;
using PerfumeStoreApi.Context.Dtos.ProdutoDTO;
using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Service.Interfaces;

public interface IProdutoService
{
    Task<IEnumerable<GetProdutosDto?>> ListarProdutosTodosAsync();
    Task<GetProdutosDto?> ObterProdutoAsync(int id);
    Task<ProdutoDto?> CriarProdutoAsync(ProdutoCreateUpdateDto produto);
    Task<ProdutoDto?> AtualizarProdutoAsync(int id, ProdutoCreateUpdateDto dadosAtualizados);
    Task<bool> ExcluirProdutoAsync(int id);
}