using BusinessWeb.Domain.Entities;
using BusinessWeb.Domain.Enums;
using BusinessWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BusinessWeb.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        if (!await db.Users.AnyAsync())
        {
            var admin = new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = UserRole.Admin
            };

            db.Users.Add(admin);
            await db.SaveChangesAsync();
        }
    }
}
