using Microsoft.EntityFrameworkCore;
using Sprint.Models;

namespace Sprint.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Moto> Motos { get; set; }
        public DbSet<Patio> Patios { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Chaveiro> Chaveiros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("RM557825"); 

            modelBuilder.Entity<Moto>().ToTable("MOTOS");
            modelBuilder.Entity<Patio>().ToTable("PATIOS");
            modelBuilder.Entity<Chaveiro>().ToTable("CHAVEIROS");
            modelBuilder.Entity<Funcionario>().ToTable("FUNCIONARIOS");

           
            modelBuilder.Entity<Moto>()
                .HasOne(m => m.Chaveiro)
                .WithOne(c => c.Moto)
                .HasForeignKey<Chaveiro>(c => c.MotoId);

            modelBuilder.Entity<Moto>()
                .HasMany(m => m.Patios)
                .WithMany(p => p.Motos)
                .UsingEntity<Dictionary<string, object>>(
                    j => j.HasOne<Patio>().WithMany().HasForeignKey("PATIOID").HasConstraintName("FK_MOTOS_PATIOS_PATIO"),
                    j => j.HasOne<Moto>().WithMany().HasForeignKey("MOTOID").HasConstraintName("FK_MOTOS_PATIOS_MOTO"),
                    j =>
                    {
                        j.HasKey("MOTOID", "PATIOID");
                        j.ToTable("MOTOS_PATIOS"); 
                    });

            base.OnModelCreating(modelBuilder);
        }
    }
}