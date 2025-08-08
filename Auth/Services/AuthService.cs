using ICTDashboard.Auth.Helpers;
using ICTDashboard.Auth.Models;
using ICTDashboard.Auth.Models.Dtos;
using ICTDashboard.Auth.Services.Interfaces;
using ICTDashboard.Core.Contexts;
using ICTDashboard.Profile.Models;
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

    public async Task<SignInResultDto> SignInAsync(SignInRequestDto requestDto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == requestDto.Email);

        var validation = AuthFormValidationHelper.ValidateSignIn(user, requestDto.Password);
        if (!validation.IsSuccess)
            return validation;

        var token = AuthJwtHelper.GenerateToken(user!, _config["Jwt:Key"]!);

        return new SignInResultDto
        {
            IsSuccess = true,
            Token = token
        };
    }

    public async Task SignUpAsync(SignUpRequestDto requestDto)
    {
        var (parsedRole, errors) = await AuthFormValidationHelper.ValidateSignUpAsync(_context, requestDto);
        if (errors.Count > 0)
            throw new ValidationException(errors);

        var user = new User
        {
            FirstName = requestDto.FirstName!.Trim(),
            LastName = requestDto.LastName!.Trim(),
            Username = requestDto.Username!.Trim(),
            Email = requestDto.Email!.Trim(),
            Role = parsedRole!.Value,
            PasswordHash = AuthPasswordHelper.Hash(requestDto.Password!),
            Profile = new UserProfile()
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}