using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Repository.EntityConfigurations
{
    public class OrderItemConfiguration : BaseModelConfiguration<OrderItem>
    {
        public override void ConfigureDerived(EntityTypeBuilder<OrderItem> builder)
        {
            builder
                .HasOne<Order>(x => x.Order)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.OrderId);

            builder
                .HasOne<Product>(x => x.Product)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.ProductId);
        }
    }
}
