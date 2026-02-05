using BusinessWeb.Application.Interfaces;
using BusinessWeb.Domain.Entities;
using BusinessWeb.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessWeb.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _config;

    public JwtTokenService(IConfiguration config) => _config = config;

    public string CreateToken(User user)
    {
        var secret = _config["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret missing");
        var lifetimeMinStr = _config["Jwt:LifetimeMinutes"] ?? "120";
        if (!int.TryParse(lifetimeMinStr, out var lifetimeMinutes)) lifetimeMinutes = 120;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role.ToString()),
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(lifetimeMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
