using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transaction = EShop.Domain.Models.Transaction;

namespace EShop.Repository.EntityConfigurations
{
    public class OrderConfiguration : BaseModelConfiguration<Order>
    {
        public override void ConfigureDerived(EntityTypeBuilder<Order> builder)
        {
            builder
                .HasOne<User>(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId);

            builder
                .HasMany<OrderItem>(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);

            builder
                .HasMany<Transaction>(x => x.Transactions)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId);


            builder.Property(x => x.UserId).IsRequired().HasMaxLength(256);
            builder.Property(x => x.IsPaid).IsRequired();
            builder.Property(x => x.TotalAmount).IsRequired();
            builder.Property(x => x.FinalAmount).IsRequired();
            builder.Property(x => x.DiscountAmount).IsRequired();

            //Indexes
            builder.HasIndex(x => x.IsPaid).HasFilter("IsDeleted=0");
        }
    }
}
