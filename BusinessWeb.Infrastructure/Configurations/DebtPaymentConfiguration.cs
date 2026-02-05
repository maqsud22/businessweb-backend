using BusinessWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessWeb.Infrastructure.Configurations;

public class DebtPaymentConfiguration : IEntityTypeConfiguration<DebtPayment>
{
    public void Configure(EntityTypeBuilder<DebtPayment> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Amount).HasPrecision(18, 2);
        b.Property(x => x.Date).IsRequired();

        b.HasOne(x => x.Debt)
            .WithMany(d => d.Payments)
            .HasForeignKey(x => x.DebtId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.Date);

        b.HasQueryFilter(x => x.DeletedAt == null);
    }
}
