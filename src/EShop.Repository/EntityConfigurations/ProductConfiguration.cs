using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Repository.EntityConfigurations
{
    public class ProductConfiguration : BaseModelConfiguration<Product>
    {
        public override void ConfigureDerived(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(256);

            //Indexes
            builder.HasIndex(x => x.Name).HasFilter("NOT is_deleted");

            builder
                .HasMany(x => x.OrderItems)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);
        }
    }
}
