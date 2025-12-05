using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SupplierProductCalculator.Entities;

namespace SupplierProductCalculator;

public class TestDbContext : DbContext
{
    private static readonly SqliteConnection _keepAliveConnection =
        new SqliteConnection("DataSource=:memory:");

    static TestDbContext()
    {
        _keepAliveConnection.Open(); // database blijft bestaan zolang de app draait
    }
    public TestDbContext() : base()
    {
        Database.EnsureCreated();
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderRow> OrderRows => Set<OrderRow>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<SupplierProduct> SupplierProducts => Set<SupplierProduct>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options
            .UseLazyLoadingProxies()
            .UseSqlite(_keepAliveConnection);
    }
    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<Order>()
            .HasKey(o => o.Id);
        model.Entity<OrderRow>()
            .HasKey(or => or.Id);
        model.Entity<Product>()
            .HasKey(o => o.Id);
        model.Entity<Supplier>()
            .HasKey(o => o.Id);
        model.Entity<SupplierProduct>()
            .HasKey(sp => sp.Id);

        // ---------- Order -> OrderRows ----------
        model.Entity<Order>()
            .HasMany(o => o.OrderRows)
            .WithOne(r => r.Order)
            .HasForeignKey(r => r.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // ---------- OrderRow -> Product ----------
        model.Entity<OrderRow>()
            .HasOne(r => r.Product)
            .WithMany(r => r.OrderRows)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // ---------- SupplierProduct (many-to-many) ----------
        model.Entity<SupplierProduct>()
            .HasOne(sp => sp.Supplier)
            .WithMany(s => s.SupplierProducts)
            .HasForeignKey(sp => sp.SupplierId)
            .OnDelete(DeleteBehavior.Cascade);

        model.Entity<SupplierProduct>()
            .HasOne(sp => sp.Product)
            .WithMany(p => p.SupplierProducts)
            .HasForeignKey(sp => sp.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        model.Entity<SupplierProduct>()
            .HasIndex(sp => new { sp.SupplierId, sp.ProductId })
            .IsUnique(false);
    }
}


// Calculator output
