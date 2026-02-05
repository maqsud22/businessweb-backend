using BusinessWeb.Domain.Enums;

namespace BusinessWeb.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public UserRole Role { get; set; }
}
