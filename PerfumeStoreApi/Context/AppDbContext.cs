using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Venda> Vendas { get; set; }
    public DbSet<ItemVenda> ItemVendas { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }
}