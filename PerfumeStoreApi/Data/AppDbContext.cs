using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Produto>()
            .Property(p => p.PrecoCompra)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Produto>()
            .Property(p => p.PrecoVenda)
            .HasPrecision(10, 2);

        modelBuilder.Entity<ItemVenda>()
            .Property(iv => iv.PrecoUnitario)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Pagamento>()
            .Property(p => p.ValorPago)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Venda>()
            .Property(v => v.ValorTotal)
            .HasPrecision(10, 2);
    }


    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Venda> Vendas { get; set; }
    public DbSet<ItemVenda> ItemVendas { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }
}