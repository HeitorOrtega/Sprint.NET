using Microsoft.EntityFrameworkCore;
using Sprint_1.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Moto> Motos { get; set; }
    public DbSet<Funcionario> Funcionarios { get; set; }
    public DbSet<Patio> Patios { get; set; }
    
    

}