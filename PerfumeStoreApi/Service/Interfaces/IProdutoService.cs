using PerfumeStoreApi.Context.Dtos;
using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Service.Interfaces;

public interface IProdutoService
{
    Task<IEnumerable<Produto>> ListarProdutosAsync();
    Task<Produto?> ObterProdutoAsync(int id);
    Task<Produto?> CriarProdutoAsync(Produto produto);
    Task<Produto?> AtualizarProdutoAsync(int id, Produto dadosAtualizados);
    Task<bool> ExcluirProdutoAsync(int id);
}