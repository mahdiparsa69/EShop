using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Repository.EntityConfigurations
{
    public class ProductConfiguration : BaseModelConfiguration<Product>
    {
        public override void ConfigureDerived(EntityTypeBuilder<Product> builder)
        {
            builder
                .HasMany<OrderItem>(x => x.OrderItems)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);

            builder.Property(x => x.Title).IsRequired().HasMaxLength(256);
            builder.Property(x => x.AvailableCount).IsRequired();
            builder.Property(x => x.Price).IsRequired();

            //Indexes
            builder.HasIndex(x => x.Title).HasFilter("IsDeleted=0");

        }
    }
}
