using System.Security.Claims;
using ICTDashboard.Auth.Models.Dtos;
using ICTDashboard.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult GetMe()
    {
        var claims = User;
        var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = claims.FindFirst(ClaimTypes.Name)?.Value;
        var email = claims.FindFirst(ClaimTypes.Email)?.Value;
        var role = claims.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            id = userId,
            username,
            email,
            role
        });
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        var result = await _auth.SignInAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(new { errors = result.Errors });
        }

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(1)
        };

        Response.Cookies.Append("access_token", result.Token!, cookieOptions);

        return Ok(new { user = result.User });
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Register([FromBody] SignUpRequest request)
    {
        var result = await _auth.SignUpAsync(request);
        return Ok(result);
    }
}