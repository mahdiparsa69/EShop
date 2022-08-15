using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Repository.EntityConfigurations
{
    public class UserConfiguration : BaseModelConfiguration<User>
    {
        public override void ConfigureDerived(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Username).IsRequired().HasMaxLength(256);
            builder.Property(x => x.Password).IsRequired().HasMaxLength(256);

            //todo add maxlengh to other fields

            //Indexes
            builder.HasIndex(x => x.Username).IsUnique().HasFilter("NOT is_deleted");

            builder
                .HasMany(x => x.Orders)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            builder
                .HasMany(x => x.Transactions)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        }
    }
}
