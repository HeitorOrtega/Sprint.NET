using Microsoft.EntityFrameworkCore;
using Sprint_1.Models;


namespace Sprint_1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Moto> Motos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Patio> Patios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Moto>()
                .HasOne(m => m.Chaveiro)
                .WithOne(c => c.Moto)
                .HasForeignKey<Chaveiro>(c => c.MotoId);

            base.OnModelCreating(modelBuilder);
        }
    }
}