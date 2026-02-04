using BusinessWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessWeb.Infrastructure.Configurations;

public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired();

        b.Property(x => x.Phone)
            .HasMaxLength(30)
            .IsRequired();

        b.HasIndex(x => x.Phone);

        b.Property(x => x.IsActive).IsRequired();

        b.HasQueryFilter(x => x.DeletedAt == null);
    }
}
