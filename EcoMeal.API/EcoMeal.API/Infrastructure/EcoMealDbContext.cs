using Microsoft.EntityFrameworkCore;
using EcoMeal.API.Entities;

namespace EcoMeal.API.Infrastructure;

public class EcoMealDbContext : DbContext
{
    public EcoMealDbContext(DbContextOptions<EcoMealDbContext> options)
        : base(options) { }
    public DbSet<User> User { get; set; }
    public DbSet<BusinessType> BusinessType { get; set; }
    public DbSet<PackageType> PackageType { get; set; }
    public DbSet<Business> Business { get; set; }
    public DbSet<Package> Package { get; set; }
    public DbSet<Order> Order { get; set; }
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
            .HasForeignKey(p => p.BusinessTypeId);

        //One Business has many Packages
        modelBuilder.Entity<Package>().HasKey(e => e.Id);
        modelBuilder.Entity<Package>()
            .HasOne(p => p.Business)
            .WithMany(p => p.Packages)
            .HasForeignKey(p => p.BusinessId);


        //one PackageType has many Packages
        modelBuilder.Entity<Package>().HasKey(e => e.Id);
        modelBuilder.Entity<Package>()
            .HasOne(p => p.PackageType)
            .WithMany(p => p.Packages)
            .HasForeignKey(p => p.PackageTypeId);

        //one User has many Orders
        modelBuilder.Entity<Order>().HasKey(e => e.Id);
        modelBuilder.Entity<Order>()
            .HasOne(u => u.User)
            .WithMany(o => o.Orders)
            .HasForeignKey(p => p.UserId);


        //one Package has many Orders
        modelBuilder.Entity<Order>().HasKey(e => e.Id);
        modelBuilder.Entity<Order>()
            .HasOne(p => p.Package)
            .WithMany(o => o.Orders)
            .HasForeignKey(p => p.PackageId);
    }

}