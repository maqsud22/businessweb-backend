using BusinessWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessWeb.Infrastructure.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.TotalAmount).HasPrecision(18, 2);
        b.Property(x => x.PaymentType).IsRequired();

        b.HasIndex(x => x.CreatedAt);
        b.HasIndex(x => x.StoreId);

        b.HasMany(x => x.Lines)
            .WithOne(l => l.Sale)
            .HasForeignKey(l => l.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasQueryFilter(x => x.DeletedAt == null);
    }
}
