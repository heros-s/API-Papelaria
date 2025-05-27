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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estoque>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<Estoque>()
            .HasOne(e => e.Material)
            .WithOne()
            .HasForeignKey<Estoque>(e => e.Id);
    }
}
