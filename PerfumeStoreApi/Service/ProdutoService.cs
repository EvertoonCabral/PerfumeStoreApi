using PerfumeStoreApi.Service.Interfaces;

namespace PerfumeStoreApi.Service;

using PerfumeStoreApi.Models;
using PerfumeStoreApi.UnitOfWork;


public class ProdutoService : IProdutoService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProdutoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Produto>> ListarProdutosTodosAsync()
    {
        return await _unitOfWork.ProdutoRepository.GetAll();
    }

    public async Task<Produto?> ObterProdutoAsync(int id)
    {
        return await _unitOfWork.ProdutoRepository.GetById(id);
    }

    public async Task<Produto?> CriarProdutoAsync(Produto produto)
    {
        if (produto.PrecoVenda < produto.PrecoCompra)
            throw new InvalidOperationException("O preço de venda não pode ser inferior ao preço de compra.");

        if (produto.QuantidadeEstoque < 0)
            throw new InvalidOperationException("A quantidade em estoque não pode ser negativa.");

        var novoProduto = _unitOfWork.ProdutoRepository.Create(produto);
        await _unitOfWork.CommitAsync();

        return novoProduto;
    }

    public async Task<Produto?> AtualizarProdutoAsync(int id, Produto dadosAtualizados)
    {
        var produtoExistente = await _unitOfWork.ProdutoRepository.GetById(id);

        if (produtoExistente == null)
            return null;

        if (!produtoExistente.IsAtivo)
            throw new InvalidOperationException("Produto inativo não pode ser alterado.");

        if (dadosAtualizados.PrecoVenda < dadosAtualizados.PrecoCompra)
            throw new InvalidOperationException("O preço de venda não pode ser inferior ao preço de compra.");

        _unitOfWork.ProdutoRepository.Update(produtoExistente, dadosAtualizados);
        await _unitOfWork.CommitAsync();

        return produtoExistente;
    }

    public async Task<bool> ExcluirProdutoAsync(int id)
    {
        var produto = await _unitOfWork.ProdutoRepository.GetById(id);

        if (produto == null)
            return false;

        _unitOfWork.ProdutoRepository.Delete(produto);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
