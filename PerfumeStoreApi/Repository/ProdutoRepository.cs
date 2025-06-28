using AutoMapper;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Repository;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    private readonly IMapper _mapper;
    
    public ProdutoRepository(AppDbContext context) : base(context)
    {
        
    }
    
    public void Update(Produto existingEntity, Produto newValues)
    {
        _context.Entry(existingEntity).CurrentValues.SetValues(newValues);
    }
}