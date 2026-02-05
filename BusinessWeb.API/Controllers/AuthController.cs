using BusinessWeb.Application.DTOs.Auth;
using BusinessWeb.Application.Interfaces;
using BusinessWeb.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusinessWeb.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenService _jwt;

    public AuthController(IUnitOfWork uow, IPasswordHasher hasher, IJwtTokenService jwt)
    {
        _uow = uow;
        _hasher = hasher;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto, CancellationToken ct)
    {
        var users = _uow.Repo<User>().Query();

        var exists = await users.AnyAsync(x => x.Username == dto.Username, ct);
        if (exists) return Conflict(new { message = "Username already exists" });

        var user = new User
        {
            Username = dto.Username.Trim(),
            PasswordHash = _hasher.Hash(dto.Password),
            Role = dto.Role
        };

        await _uow.Repo<User>().AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        var token = _jwt.CreateToken(user);

        return Ok(new AuthResponseDto
        {
            UserId = user.Id,
            Username = user.Username,
            Role = user.Role,
            Token = token
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto, CancellationToken ct)
    {
        var user = await _uow.Repo<User>().Query()
            .FirstOrDefaultAsync(x => x.Username == dto.Username, ct);

        if (user is null) return Unauthorized(new { message = "Invalid credentials" });
        if (!_hasher.Verify(dto.Password, user.PasswordHash)) return Unauthorized(new { message = "Invalid credentials" });

        var token = _jwt.CreateToken(user);

        return Ok(new AuthResponseDto
        {
            UserId = user.Id,
            Username = user.Username,
            Role = user.Role,
            Token = token
        });
    }
}
