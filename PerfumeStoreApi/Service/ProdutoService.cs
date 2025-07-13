using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Context.Dtos.ProdutoDTO;
using PerfumeStoreApi.Data.Dtos.Produto;
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

    public async Task<IEnumerable<GetProdutosDto?>> ListarProdutosTodosAsync()
    {
        var produtos=  await _unitOfWork.ProdutoRepository.GetAll();

        if (produtos is null)
        {
            throw new NullReferenceException("Produtos não encontrados");
        }
        
        return _mapper.Map<IEnumerable<GetProdutosDto>>(produtos);
    }

    public async Task<GetProdutosDto?> ObterProdutoAsync(int id)
    {
        var produto = await _unitOfWork.ProdutoRepository
            .GetByCondition(p => p.Id == id)
            .Include(p => p.ItensEstoque)
            .ThenInclude(ie => ie.Estoque)
            .FirstOrDefaultAsync();
    
        if (produto is null)
        {
            return null;
        }
    
        var produtoDto = _mapper.Map<GetProdutosDto>(produto);
    
      // pegar o primeiro estoque
        var primeiroEstoque = produto.ItensEstoque.FirstOrDefault();
        if (primeiroEstoque != null)
        {
            // Assumindo que você tem uma propriedade EstoqueId no DTO
             produtoDto.EstoqueId = primeiroEstoque.EstoqueId;
        }
    
        return produtoDto;
    }

    public async Task<ProdutoDto?> CriarProdutoAsync(ProdutoCreateUpdateDto produtoDto)
    {
        if (produtoDto.PrecoVenda < produtoDto.PrecoCompra)
            throw new InvalidOperationException("O preço de venda não pode ser inferior ao preço de compra.");
        
        var produto = _mapper.Map<Produto>(produtoDto);
        

        var novoProduto = _unitOfWork.ProdutoRepository.Create(produto);
        await _unitOfWork.CommitAsync();
        
        var produtoResult =  _mapper.Map<ProdutoDto>(novoProduto);

        return produtoResult;
    }

    public async Task<ProdutoDto?> AtualizarProdutoAsync(int id, ProdutoCreateUpdateDto dto)
    {
        var produtoExistente = await _unitOfWork.ProdutoRepository.GetById(id);
        if (produtoExistente == null)
            return null;

        if (!produtoExistente.IsAtivo)
            throw new InvalidOperationException("Produto inativo não pode ser alterado.");

        if (dto.PrecoVenda < dto.PrecoCompra)
            throw new InvalidOperationException("O preço de venda não pode ser inferior ao preço de compra.");

        _mapper.Map(dto, produtoExistente);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<ProdutoDto>(produtoExistente);
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
