using BusinessWeb.Domain.Enums;

namespace BusinessWeb.Application.DTOs.Auth;

public class AuthResponseDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
    public UserRole Role { get; set; }
    public string Token { get; set; } = null!;
}
