using System.Security.Claims;
using ICTDashboard.Auth.Models.Dtos;
using ICTDashboard.Auth.Services.Interfaces;
using ICTDashboard.Core.Contexts;
using ICTDashboard.Profile.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ICTDashboard.Auth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetMe([FromServices] IctDbContext db)
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id))
            return Unauthorized();

        var user = await db.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return Unauthorized();

        return Ok(new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString(),
            Profile = new UserProfileDto
            {
                PictureUrl = user.Profile.PictureUrl,
                Birthday = user.Profile.Birthday,
                Bio = user.Profile.Bio
            }
        });
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequestDto requestDto)
    {
        var result = await _auth.SignInAsync(requestDto);

        if (!result.IsSuccess)
        {
            return BadRequest(new { errors = result.Errors });
        }

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTimeOffset.UtcNow.AddHours(1),
            Path = "/"
        };

        Response.Cookies.Append("access_token", result.Token!, cookieOptions);
        return NoContent();
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Register([FromBody] SignUpRequestDto requestDto)
    {
        await _auth.SignUpAsync(requestDto);
        return NoContent();
    }
}