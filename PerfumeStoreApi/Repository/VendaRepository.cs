using AutoMapper;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Data;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.Repository.Interface;

namespace PerfumeStoreApi.Repository;

public class VendaRepository(AppDbContext context) : Repository<Venda>(context), IVendaRepository
{

    private readonly IMapper _mapper;
}