using ICTDashboard.Auth.Enums;
using ICTDashboard.Auth.Helpers;
using ICTDashboard.Auth.Models.Dtos;
using ICTDashboard.Core.Contexts;
using ICTDashboard.Core.Models;
using ICTDashboard.Core.Models.Dtos;
using ICTDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ICTDashboard.Auth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IctDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(IctDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("signin")]
    public async Task<IActionResult> Login([FromBody] SignInRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !PasswordHelper.Verify(user.PasswordHash, request.Password))
        {
            var error = new ErrorResponse();
            error.Errors.Add(new ErrorDetail
            {
                Field = "credentials",
                Message = "Invalid email or password."
            });
            return BadRequest(error);
        }

        var jwtSecret = _config["Jwt:Key"]!;

        var token = JwtHelper.GenerateToken(user, jwtSecret);

        return Ok(new SignInResponse
        {
            Token = token,
            User = new SignUpResponse()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString()
            }
        });
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Register([FromBody] SignUpRequest request)
    {
        var errorResponse = new ErrorResponse();

        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            errorResponse.Errors.Add(new ErrorDetail { Field = "email", Message = "Email already exists." });

        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            errorResponse.Errors.Add(new ErrorDetail { Field = "username", Message = "Username already exists." });

        if (errorResponse.Errors.Any())
            return BadRequest(errorResponse);

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = PasswordHelper.Hash(request.Password),
            Role = UserRole.Coach
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var response = new SignUpResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role.ToString()
        };

        return Ok(response);
    }
}