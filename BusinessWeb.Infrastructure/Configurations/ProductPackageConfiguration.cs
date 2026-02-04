using BusinessWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessWeb.Infrastructure.Configurations;

public class ProductPackageConfiguration : IEntityTypeConfiguration<ProductPackage>
{
    public void Configure(EntityTypeBuilder<ProductPackage> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Name)
            .HasMaxLength(80)
            .IsRequired();

        b.Property(x => x.Multiplier)
            .HasPrecision(18, 4);

        b.HasIndex(x => new { x.ProductId, x.Name }).IsUnique();

        b.HasQueryFilter(x => x.DeletedAt == null);
    }
}
