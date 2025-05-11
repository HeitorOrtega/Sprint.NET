using Microsoft.EntityFrameworkCore;
using Sprint_1.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Moto> Motos { get; set; }
    public DbSet<Chaveiro> Chaveiros { get; set; }
    public DbSet<Patio> Patios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}