using ICTDashboard.Auth.Helpers;
using ICTDashboard.Auth.Models;
using ICTDashboard.Auth.Models.Dtos;
using ICTDashboard.Auth.Services.Interfaces;
using ICTDashboard.Core.Contexts;
using ICTDashboard.Core.Models;
using Microsoft.EntityFrameworkCore;
using SignInResult = ICTDashboard.Auth.Models.Dtos.SignInResult;
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

    public async Task<SignInResult> SignInAsync(SignInRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !PasswordHelper.Verify(user.PasswordHash, request.Password))
        {
            return new SignInResult
            {
                IsSuccess = false,
                Errors = new[]
                {
                    new ErrorDetail { Field = "credentials", Message = "Invalid email or password." }
                }
            };
        }

        var token = JwtHelper.GenerateToken(user, _config["Jwt:Key"]!);

        return new SignInResult
        {
            IsSuccess = true,
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

        if (request.Password != request.ConfirmPassword)
            errors.Add(new ErrorDetail { Field = "confirmPassword", Message = "Passwords do not match." });

        if (errors.Any())
            throw new ValidationException(errors);

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = PasswordHelper.Hash(request.Password),
            Role = request.Role
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