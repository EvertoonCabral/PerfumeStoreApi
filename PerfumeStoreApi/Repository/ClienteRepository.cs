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


    public async Task<ActionResult<ClienteDetalhesDto>> RetornaClienteDetalhesAsync(int id)
    {
        var cliente = await _context.Clientes
            .Include(c => c.Vendas)
            .FirstOrDefaultAsync(c => c.Id == id);
        return _mapper.Map<ClienteDetalhesDto>(cliente);
    }
    public async Task<Cliente?> GetByCpfAsync(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return null;

        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Cpf == cpf && c.IsAtivo);
    }
    
    
}