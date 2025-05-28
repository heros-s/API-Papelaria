// AppDbContext.cs

using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    { }

    public DbSet<Material> Materiais { get; set; }
    public DbSet<Estoque> Estoques { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Carrinho> Carrinhos { get; set; }
    public DbSet<ItemCarrinho> ItensCarrinho { get; set; }
    public DbSet<Venda> Vendas { get; set; }
    public DbSet<ItemVenda> ItensVenda { get; set; }
    public DbSet<ContaPagar> ContasPagar { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estoque>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Estoque>()
            .HasOne(e => e.Material)
            .WithOne()
            .HasForeignKey<Estoque>(e => e.Id);
        modelBuilder.Entity<ItemCarrinho>()
            .HasOne(i => i.Material)
            .WithMany()
            .HasForeignKey(i => i.MaterialId);

        modelBuilder.Entity<ItemCarrinho>()
            .HasOne(i => i.Carrinho)
            .WithMany(c => c.Itens)
            .HasForeignKey(i => i.CarrinhoId);

        modelBuilder.Entity<ItemVenda>()
            .HasOne(iv => iv.Material)
            .WithMany()
            .HasForeignKey(iv => iv.MaterialId);

        modelBuilder.Entity<ItemVenda>()
            .HasOne(iv => iv.Venda)
            .WithMany(v => v.Itens)
            .HasForeignKey(iv => iv.VendaId);

        modelBuilder.Entity<Venda>()
            .Property(v => v.Status)
            .HasConversion<string>();

    }
}
