using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transaction = EShop.Domain.Models.Transaction;

namespace EShop.Repository.EntityConfigurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder
                .HasOne<Order>(x => x.Order)
                .WithMany(x => x.Transanctions)
                .HasForeignKey(x => x.OrderId);
        }
    }
}
