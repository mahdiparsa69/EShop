using System.Xml.Schema;
using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Repository.EntityConfigurations
{
    public class TransactionConfiguration : BaseModelConfiguration<Transaction>
    {
        public override void ConfigureDerived(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.OrderId).IsRequired();

            //Indexes

            builder.HasIndex(p => new { p.OrderId, p.Status }).HasFilter("NOT is_deleted AND status=1").IsUnique();


            builder
                .HasOne(x => x.Order)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.OrderId);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.UserId);


        }
    }
}
