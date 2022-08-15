using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Repository.EntityConfigurations
{
    public class TransactionConfiguration : BaseModelConfiguration<Transaction>
    {
        public override void ConfigureDerived(EntityTypeBuilder<Transaction> builder)
        {
            builder
                .HasOne<Order>(x => x.Order)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.OrderId);

            builder
                .HasOne<User>(x => x.User)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.UserId);

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.OrderId).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.HasError).IsRequired();

            //Indexes
            builder.HasIndex(x => x.Status).HasFilter("IsDeleted=0").IsUnique();
        }
    }
}
