using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Data;
using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Repository;

public class ItemEstoqueRepository : Repository<ItemEstoque>, IItemEstoqueRepository
{

    private readonly IMapper _mapper;
    public ItemEstoqueRepository(AppDbContext context) : base(context)
    {
        
    }
    
    public async Task<ItemEstoque?> ObterPorProdutoEEstoqueAsync(int produtoId, int estoqueId)
    {
        return await _dbSet
            .Include(ie => ie.Produto)
            .Include(ie => ie.Estoque)
            .FirstOrDefaultAsync(ie => ie.ProdutoId == produtoId && ie.EstoqueId == estoqueId);
    }

    public async Task<IEnumerable<ItemEstoque>> ObterPorProdutoAsync(int produtoId)
    {
        return await _dbSet
            .Include(ie => ie.Estoque)
            .Where(ie => ie.ProdutoId == produtoId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ItemEstoque>> ObterPorEstoqueAsync(int estoqueId)
    {
        return await _dbSet
            .Include(ie => ie.Produto)
            .Where(ie => ie.EstoqueId == estoqueId)
            .ToListAsync();
    }
    
    
}

    
