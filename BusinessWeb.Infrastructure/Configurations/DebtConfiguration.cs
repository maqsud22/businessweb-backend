using BusinessWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessWeb.Infrastructure.Configurations;

public class DebtConfiguration : IEntityTypeConfiguration<Debt>
{
    public void Configure(EntityTypeBuilder<Debt> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Total).HasPrecision(18, 2);
        b.Property(x => x.Paid).HasPrecision(18, 2);
        b.Property(x => x.IsClosed).IsRequired();

        b.HasIndex(x => x.SaleId).IsUnique();

        b.HasMany(x => x.Payments)
            .WithOne()
            .HasForeignKey(p => p.DebtId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasQueryFilter(x => x.DeletedAt == null);
    }
}
