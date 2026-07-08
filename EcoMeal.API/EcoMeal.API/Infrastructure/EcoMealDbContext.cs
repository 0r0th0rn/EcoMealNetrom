using Microsoft.EntityFrameworkCore;
using EcoMeal.API.Entities;

namespace EcoMeal.API.Infrastructure;

public class EcoMealDbContext : DbContext
{
    public EcoMealDbContext(DbContextOptions<EcoMealDbContext> options)
        : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<BusinessType> BusinessTypes { get; set; }
    public DbSet<PackageType> PackageTypes { get; set; }
    public DbSet<Business> Businesses { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //solved warn about Price precision
        modelBuilder.Entity<Package>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);
        //one BusinessType has many Businesses
        modelBuilder.Entity<Business>().HasKey(e => e.Id);
        modelBuilder.Entity<Business>()
            .HasOne(p => p.BusinessType)
            .WithMany(p => p.Businesses)
            .HasForeignKey(p => p.BusinessTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        //One Business has many Packages
        modelBuilder.Entity<Package>().HasKey(e => e.Id);
        modelBuilder.Entity<Package>()
            .HasOne(p => p.Business)
            .WithMany(p => p.Packages)
            .HasForeignKey(p => p.BusinessId)
            .OnDelete(DeleteBehavior.Cascade);


        //one PackageType has many Packages
        modelBuilder.Entity<Package>()
            .HasOne(p => p.PackageType)
            .WithMany(p => p.Packages)
            .HasForeignKey(p => p.PackageTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        //one User has many Orders
        modelBuilder.Entity<Order>().HasKey(e => e.Id);
        modelBuilder.Entity<Order>()
            .HasOne(u => u.User)
            .WithMany(o => o.Orders)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);


        //one Package has many Orders
        modelBuilder.Entity<Order>()
            .HasOne(p => p.Package)
            .WithMany(o => o.Orders)
            .HasForeignKey(p => p.PackageId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}