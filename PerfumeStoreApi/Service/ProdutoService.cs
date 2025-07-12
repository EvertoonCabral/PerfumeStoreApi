using AutoMapper;
using PerfumeStoreApi.Context.Dtos.ProdutoDTO;
using PerfumeStoreApi.Service.Interfaces;

namespace PerfumeStoreApi.Service;

using PerfumeStoreApi.Models;
using PerfumeStoreApi.UnitOfWork;


public class ProdutoService : IProdutoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper  _mapper;

    public ProdutoService(IUnitOfWork unitOfWork,  IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetProdutosDto>> ListarProdutosTodosAsync()
    {
        var produtos=  await _unitOfWork.ProdutoRepository.GetAll();
        return _mapper.Map<IEnumerable<GetProdutosDto>>(produtos);
    }

    public async Task<Produto?> ObterProdutoAsync(int id)
    {
        return await _unitOfWork.ProdutoRepository.GetById(id);
    }

    public async Task<ProdutoDto?> CriarProdutoAsync(ProdutoCreateUpdateDto produtoDto)
    {
        if (produtoDto.PrecoVenda < produtoDto.PrecoCompra)
            throw new InvalidOperationException("O preço de venda não pode ser inferior ao preço de compra.");

        if (produtoDto.QuantidadeEstoque < 0)
            throw new InvalidOperationException("A quantidade em estoque não pode ser negativa.");
        
        var produto = _mapper.Map<Produto>(produtoDto);
        
        if (produto.EstoqueId == 0 || produto.EstoqueId == null) // considerando que 0 significa não informado
        {
            produto.EstoqueId = 1;
        }

        var novoProduto = _unitOfWork.ProdutoRepository.Create(produto);
        await _unitOfWork.CommitAsync();
        
        var produtoResult =  _mapper.Map<ProdutoDto>(novoProduto);

        return produtoResult;
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
