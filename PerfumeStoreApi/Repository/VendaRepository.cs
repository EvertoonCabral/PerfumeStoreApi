using AutoMapper;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Repository;

public class VendaRepository : Repository<Venda>, IVendaRepository 
{

    private readonly IMapper _mapper;
    public VendaRepository(AppDbContext context) : base(context)
    {
        
    }

    
}