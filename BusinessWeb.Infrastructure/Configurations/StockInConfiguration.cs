using BusinessWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessWeb.Infrastructure.Configurations;

public class StockInConfiguration : IEntityTypeConfiguration<StockIn>
{
    public void Configure(EntityTypeBuilder<StockIn> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Quantity).HasPrecision(18, 4);
        b.Property(x => x.CostPrice).HasPrecision(18, 2);
        b.Property(x => x.Date).IsRequired();

        b.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ProductPackage)
            .WithMany()
            .HasForeignKey(x => x.ProductPackageId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasIndex(x => x.Date);

        b.HasQueryFilter(x => x.DeletedAt == null);
    }
}
