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

            builder.Property(x => x.OrderId).IsRequired();
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.Count).IsRequired();
            builder.Property(x => x.ProductPrice).IsRequired();
            builder.Property(x => x.TotalPrice).IsRequired();


        }
    }
}
