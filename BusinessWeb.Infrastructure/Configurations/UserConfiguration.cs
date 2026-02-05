using BusinessWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessWeb.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Username)
            .HasMaxLength(64)
            .IsRequired();

        b.HasIndex(x => x.Username).IsUnique();

        b.Property(x => x.PasswordHash)
            .HasMaxLength(256)
            .IsRequired();

        b.Property(x => x.Role).IsRequired();

        // soft delete filter
        b.HasQueryFilter(x => x.DeletedAt == null);
    }
}
