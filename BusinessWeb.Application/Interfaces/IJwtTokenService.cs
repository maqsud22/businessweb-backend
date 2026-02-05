using BusinessWeb.Domain.Entities;

namespace BusinessWeb.Application.Interfaces;

public interface IJwtTokenService
{
    string CreateToken(User user);
}
