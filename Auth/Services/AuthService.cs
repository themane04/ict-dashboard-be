using ICTDashboard.Auth.Helpers;
using ICTDashboard.Auth.Models;
using ICTDashboard.Auth.Models.Dtos;
using ICTDashboard.Auth.Services.Interfaces;
using ICTDashboard.Core.Contexts;
using ICTDashboard.Profile.Models;
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
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        var validation = FormValidationHelper.ValidateSignIn(user, request.Password);
        if (!validation.IsSuccess)
            return validation;

        var token = JwtHelper.GenerateToken(user!, _config["Jwt:Key"]!);

        return new SignInResult
        {
            IsSuccess = true,
            Token = token,
            User = new SignUpResponse
            {
                Id = user!.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString()
            }
        };
    }

    public async Task<SignUpResponse> SignUpAsync(SignUpRequest request)
    {
        var (parsedRole, errors) = await FormValidationHelper.ValidateSignUpAsync(_context, request);

        if (errors.Count > 0)
            throw new ValidationException(errors);

        var user = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Username = request.Username.Trim(),
            Email = request.Email.Trim(),
            Role = parsedRole!.Value,
            PasswordHash = PasswordHelper.Hash(request.Password),
            Profile = new UserProfile()
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new SignUpResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}