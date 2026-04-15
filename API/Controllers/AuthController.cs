using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly WCupDbContext _context;

    public AuthController(WCupDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
        {
            return BadRequest("Kullanıcı adı zorunludur.");
        }

        var normalizedUsername = request.Username.Trim();
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == normalizedUsername);

        if (user is null)
        {
            user = new User { Username = normalizedUsername };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        return new UserDto(user.Id, user.Username);
    }
}

public record LoginRequest(string Username);
public record UserDto(int Id, string Username);
