using EShop.Domain.Models;
using EShop.Repository.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace EShop.Repository;

public class EShopDbContext : DbContext
{
    //Ctor
    public EShopDbContext(DbContextOptions<EShopDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        /*base.OnModelCreating(modelBuilder);
         var assembly = typeof(ProductConfiguration).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);*/
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        /*optionsBuilder.UseSnakeCaseNamingConvention();
        base.OnConfiguring(optionsBuilder);*/
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<Transaction> Transactions { get; set; }
}
