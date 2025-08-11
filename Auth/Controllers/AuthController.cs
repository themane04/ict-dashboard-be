using ICTDashboard.Auth.Extensions;
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
    public async Task<ActionResult<UserDto>> GetMe()
    {
        var id = User.GetUserId();
        if (id is null) return Unauthorized();

        var dto = await _auth.GetMeAsync(id.Value);
        if (dto is null) return Unauthorized();

        return Ok(dto);
    }

    [HttpPost("signout")]
    public new IActionResult SignOut()
    {
        Response.Cookies.Delete("access_token", new CookieOptions
        {
            Path = "/",
            SameSite = SameSiteMode.None,
            Secure = true
        });
        return NoContent();
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
            Expires = DateTimeOffset.UtcNow.AddDays(7),
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