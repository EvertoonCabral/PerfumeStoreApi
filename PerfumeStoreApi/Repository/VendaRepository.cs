using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Data;
using PerfumeStoreApi.Models;
using PerfumeStoreApi.Repository.Interface;

namespace PerfumeStoreApi.Repository;

public class VendaRepository(AppDbContext context) : Repository<Venda>(context), IVendaRepository
{
    public IQueryable<Venda> Query()
        => _context.Set<Venda>().AsNoTracking();
}