using EShop.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Repository.EntityConfigurations
{
    public abstract class BaseModelConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseModel
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.SeqId)
                .UseIdentityColumn()
                .ValueGeneratedOnAdd();

            builder.HasQueryFilter(x => !x.IsDeleted);

            builder.HasIndex(x => x.SeqId).IsUnique();

            builder.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");

            ConfigureDerived(builder);
        }

        public abstract void ConfigureDerived(EntityTypeBuilder<TEntity> builder);
    }
}
