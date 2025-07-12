using PerfumeStoreApi.Context.Dtos;
using PerfumeStoreApi.Context.Dtos.ProdutoDTO;
using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Service.Interfaces;

public interface IProdutoService
{
    Task<IEnumerable<GetProdutosDto>> ListarProdutosTodosAsync();
    Task<Produto?> ObterProdutoAsync(int id);
    Task<ProdutoDto?> CriarProdutoAsync(ProdutoCreateUpdateDto produto);
    Task<Produto?> AtualizarProdutoAsync(int id, Produto dadosAtualizados);
    Task<bool> ExcluirProdutoAsync(int id);
}