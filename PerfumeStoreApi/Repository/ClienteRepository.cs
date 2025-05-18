using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Context;
using PerfumeStoreApi.Context.Dtos;
using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Repository;

public class ClienteRepository : Repository<Cliente>, IClienteRepository
{
    private readonly IMapper _mapper;
    public ClienteRepository(AppDbContext context) : base(context)
    {
        
    }


    public ActionResult<ClienteDetalhesDto> RetornaClienteDetalhes(int id)
    {
        var cliente =  _context.Clientes
            .Include(c => c.Vendas)
            .FirstOrDefaultAsync(c => c.Id == id);
        return _mapper.Map<ClienteDetalhesDto>(cliente);
    }
}