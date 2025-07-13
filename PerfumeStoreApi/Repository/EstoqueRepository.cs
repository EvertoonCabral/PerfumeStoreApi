using AutoMapper;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Data;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.Repository.Interface;

namespace PerfumeStoreApi.Repository;

public class EstoqueRepository : Repository<Estoque>, IEstoqueRepository
{
    
    private readonly IMapper _mapper;
    public EstoqueRepository(AppDbContext context) : base(context)
    {
        
    }
}
    
