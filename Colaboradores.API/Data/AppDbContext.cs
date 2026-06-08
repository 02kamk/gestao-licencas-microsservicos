using Colaboradores.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Colaboradores.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Colaborador> Colaboradores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Colaborador>()
            .HasIndex(c => c.Email)
            .IsUnique();
    }
}