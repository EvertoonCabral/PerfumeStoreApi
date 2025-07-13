using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Models;

namespace PerfumeStoreApi.Data;

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

        modelBuilder.Entity<Pagamento>()
            .Property(p => p.ValorPago)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Venda>()
            .Property(v => v.ValorTotal)
            .HasPrecision(10, 2);
        
        
        modelBuilder.Entity<Estoque>().HasData(
            new Estoque
            {
                Id = 1,
                Nome = "Estoque de Novos",
                Descricao = "Produtos novos",
                DataCriacao = new DateTime(2025, 7, 12, 0, 0, 0) // valor fixo
            },
            new Estoque
            {
                Id = 2,
                Nome = "Estoque de Usados",
                Descricao = "Produtos seminovos ou usados",
                DataCriacao = new DateTime(2025, 7, 12, 0, 0, 0) // valor fixo
            }
        );
        
    }


    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Venda> Vendas { get; set; }
    public DbSet<ItemVenda> ItemVendas { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }
    public DbSet<Estoque> Estoques { get; set; }
    public DbSet<ItemEstoque> ItemEstoque { get; set; }
    public DbSet<MovimentacaoEstoque> MovimentacaoEstoque { get; set; }
    
}