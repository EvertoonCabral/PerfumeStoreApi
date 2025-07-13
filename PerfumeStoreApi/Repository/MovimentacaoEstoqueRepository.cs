using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.UnitOfWork;

namespace PerfumeStoreApi.Repository;

public class MovimentacaoEstoqueRepository : Repository<MovimentacaoEstoque>, IMovimentacaoEstoqueRepository
{
    
    private readonly IMapper _mapper;

    public MovimentacaoEstoqueRepository(AppDbContext context) : base(context)
    {
        
    }
    
    public async Task<IEnumerable<MovimentacaoEstoque>> ObterPorProdutoAsync(int produtoId, DateTime? dataInicio = null, DateTime? dataFim = null)
    {
        var query = _dbSet
            .Include(m => m.ItemEstoque)
            .ThenInclude(ie => ie.Produto)
            .Include(m => m.ItemEstoque)
            .ThenInclude(ie => ie.Estoque)
            .Where(m => m.ItemEstoque.ProdutoId == produtoId);

        if (dataInicio.HasValue)
            query = query.Where(m => m.DataMovimentacao >= dataInicio.Value);

        if (dataFim.HasValue)
            query = query.Where(m => m.DataMovimentacao <= dataFim.Value);

        return await query
            .OrderByDescending(m => m.DataMovimentacao)
            .ToListAsync();
    }
    
}