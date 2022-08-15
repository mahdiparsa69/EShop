using EShop.Domain.Models;
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
        }
    }
}
