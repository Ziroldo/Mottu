using Microsoft.EntityFrameworkCore;
using Mottu.Backend.Models;

namespace Mottu.Backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Moto> Motos => Set<Moto>();
    public DbSet<Deliverer> Deliverers => Set<Deliverer>();
    public DbSet<Rental> Rentals => Set<Rental>();
    public DbSet<MotoCreatedLog> MotoCreatedLogs => Set<MotoCreatedLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Moto>().HasIndex(m => m.Placa).IsUnique();
        modelBuilder.Entity<Moto>().HasIndex(m => m.Placa).IsUnique();
        modelBuilder.Entity<Deliverer>().HasIndex(d => d.CNPJ).IsUnique();
        modelBuilder.Entity<Deliverer>().HasIndex(d => d.CNHNumber).IsUnique();
       


        base.OnModelCreating(modelBuilder);

        
        modelBuilder.Entity<Moto>().HasIndex(m => m.Placa).IsUnique();
        modelBuilder.Entity<Deliverer>().HasIndex(d => d.CNPJ).IsUnique();
        modelBuilder.Entity<Deliverer>().HasIndex(d => d.CNHNumber).IsUnique();
modelBuilder.Entity<Moto>()
            .HasIndex(m => m.Placa)
            .IsUnique();

        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Moto)
            .WithMany()
            .HasForeignKey(r => r.MotoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Deliverer)
            .WithMany()
            .HasForeignKey(r => r.DelivererId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}


