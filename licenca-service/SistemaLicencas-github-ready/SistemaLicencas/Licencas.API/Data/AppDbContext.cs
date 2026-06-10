using Licencas.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Licencas.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Licenca> Licencas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Licenca>()
            .HasIndex(l => l.Chave)
            .IsUnique();

        modelBuilder.Entity<Licenca>()
            .HasIndex(l => l.ColaboradorId);

        modelBuilder.Entity<Licenca>()
            .Property(l => l.Status)
            .HasConversion<string>();
    }
}
