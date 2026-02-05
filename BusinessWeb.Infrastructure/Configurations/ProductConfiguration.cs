using BusinessWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessWeb.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired();

        b.Property(x => x.DefaultPrice)
            .HasPrecision(18, 2);

        b.Property(x => x.Unit).IsRequired();

        b.HasMany(x => x.Packages)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasQueryFilter(x => x.DeletedAt == null);
    }
}
