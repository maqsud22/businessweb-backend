using BusinessWeb.Domain.Enums;

namespace BusinessWeb.Application.DTOs.Auth;

public class RegisterDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserRole Role { get; set; } = UserRole.Operator;
}
