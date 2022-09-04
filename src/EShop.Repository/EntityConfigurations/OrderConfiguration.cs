using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Repository.EntityConfigurations
{
    public class OrderConfiguration : BaseModelConfiguration<Order>
    {
        public override void ConfigureDerived(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.UserId).IsRequired().HasMaxLength(256);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId);

            builder
                .HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);

            builder
                .HasMany(x => x.Transactions)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);
        }
    }
}
