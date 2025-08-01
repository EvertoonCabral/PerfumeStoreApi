﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PerfumeStoreApi.Data;

#nullable disable

namespace PerfumeStoreApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PerfumeStoreApi.Models.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cpf")
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAtivo")
                        .HasColumnType("bit");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("PerfumeStoreApi.Models.Estoque", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAtivo")
                        .HasColumnType("bit");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Estoques");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DataCriacao = new DateTime(2025, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Descricao = "Produtos novos",
                            IsAtivo = true,
                            Nome = "Estoque de Novos"
                        },
                        new
                        {
                            Id = 2,
                            DataCriacao = new DateTime(2025, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Descricao = "Produtos seminovos ou usados",
                            IsAtivo = true,
                            Nome = "Estoque de Usados"
                        });
                });

            modelBuilder.Entity("PerfumeStoreApi.Models.ItemEstoque", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataUltimaMovimentacao")
                        .HasColumnType("datetime2");

                    b.Property<int>("EstoqueId")
                        .HasColumnType("int");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("int");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.Property<int?>("QuantidadeMaxima")
                        .HasColumnType("int");

                    b.Property<int?>("QuantidadeMinima")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EstoqueId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("ItemEstoque");
                });

            modelBuilder.Entity("PerfumeStoreApi.Models.ItemVenda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProdutoId")
                        .HasColumnType("int");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.Property<int>("VendaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoId");

                    b.HasIndex("VendaId");

                    b.ToTable("ItemVendas");
                });

            modelBuilder.Entity("PerfumeStoreApi.Models.MovimentacaoEstoque", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataMovimentacao")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ItemEstoqueId")
                        .HasColumnType("int");

                    b.Property<string>("Observacoes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.Property<int>("QuantidadeAnterior")
                        .HasColumnType("int");

                    b.Property<int>("QuantidadePosterior")
                        .HasColumnType("int");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.Property<string>("UsuarioResponsavel")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ItemEstoqueId");

                    b.ToTable("MovimentacaoEstoque");
                });

            modelBuilder.Entity("PerfumeStoreApi.Models.Pagamento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataPagamento")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("ValorPago")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("VendaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VendaId");

                    b.ToTable("Pagamentos");
                });

            modelBuilder.Entity("PerfumeStoreApi.Models.Produto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsAtivo")
                        .HasColumnType("bit");

                    b.Property<string>("Marca")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PrecoCompra")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<decimal>("PrecoVenda")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("Id");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("PerfumeStoreApi.Models.Venda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClienteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataVenda")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("ValorTotal")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("Vendas");
                });

            modelBuilder.Entity("PerfumeStoreApi.Models.ItemEstoque", b =>
                {
                    b.HasOne("PerfumeStoreApi.Models.Estoque", "Estoque")
                        .WithMany("ItensEstoque")
                        .HasForeignKey("EstoqueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PerfumeStoreApi.Models.Produto", "Produto")
                        .WithMany("ItensEstoque")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estoque");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("PerfumeStoreApi.Models.ItemVenda", b =>
                {
                    b.HasOne("PerfumeStoreApi.Models.Produto", "Produto")
                        .WithMany()
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PerfumeStoreApi.Models.Venda", "Venda")
                        .WithMany("Itens")
                        .HasForeignKey("VendaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Produto");

                    b.Navigation("Venda");
                });

            modelBuilder.Entity("PerfumeStoreApi.Models.MovimentacaoEstoque", b =>
                {
                    b.HasOne("PerfumeStoreApi.Models.ItemEstoque", "ItemEstoque")
                        .WithMany("Movimentacoes")
                        .HasForeignKey("ItemEstoqueId");

                    b.Navigation("ItemEstoque");
                });

            modelBuilder.Entity("PerfumeStoreApi.Models.Pagamento", b =>
                {
                    b.HasOne("PerfumeStoreApi.Models.Venda", "Venda")
                        .WithMany("Pagamentos")
                        .HasForeignKey("VendaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Venda");
                });

            modelBuilder.Entity("PerfumeStoreApi.Models.Venda", b =>
                {
                    b.HasOne("PerfumeStoreApi.Models.Cliente", "Cliente")
                        .WithMany("Vendas")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("PerfumeStoreApi.Models.Cliente", b =>
                {
                    b.Navigation("Vendas");
                });

            modelBuilder.Entity("PerfumeStoreApi.Models.Estoque", b =>
                {
                    b.Navigation("ItensEstoque");
                });

            modelBuilder.Entity("PerfumeStoreApi.Models.ItemEstoque", b =>
                {
                    b.Navigation("Movimentacoes");
                });

            modelBuilder.Entity("PerfumeStoreApi.Models.Produto", b =>
                {
                    b.Navigation("ItensEstoque");
                });

            modelBuilder.Entity("PerfumeStoreApi.Models.Venda", b =>
                {
                    b.Navigation("Itens");

                    b.Navigation("Pagamentos");
                });
#pragma warning restore 612, 618
        }
    }
}
