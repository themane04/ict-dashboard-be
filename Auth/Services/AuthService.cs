using ICTDashboard.Auth.Enums;
using ICTDashboard.Auth.Helpers;
using ICTDashboard.Auth.Models.Dtos;
using ICTDashboard.Auth.Services.Interfaces;
using ICTDashboard.Core.Contexts;
using ICTDashboard.Core.Models;
using ICTDashboard.Models;
using Microsoft.EntityFrameworkCore;
using ValidationException = ICTDashboard.Auth.Exceptions.ValidationException;

namespace ICTDashboard.Auth.Services;

public class AuthService : IAuthService
{
    private readonly IctDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(IctDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<SignInResponse> SignInAsync(SignInRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !PasswordHelper.Verify(user.PasswordHash, request.Password))
        {
            throw new ValidationException(new List<ErrorDetail>
            {
                new() { Field = "credentials", Message = "Invalid email or password." }
            });
        }

        var token = JwtHelper.GenerateToken(user, _config["Jwt:Key"]!);

        return new SignInResponse
        {
            Token = token,
            User = new SignUpResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString()
            }
        };
    }

    public async Task<SignUpResponse> SignUpAsync(SignUpRequest request)
    {
        var errors = new List<ErrorDetail>();

        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            errors.Add(new ErrorDetail { Field = "email", Message = "Email already exists." });

        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            errors.Add(new ErrorDetail { Field = "username", Message = "Username already exists." });

        if (errors.Any())
            throw new ValidationException(errors);

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = PasswordHelper.Hash(request.Password),
            Role = UserRole.Coach
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new SignUpResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}