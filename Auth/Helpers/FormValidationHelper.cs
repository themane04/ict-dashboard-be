using ICTDashboard.Auth.Enums;
using ICTDashboard.Auth.Models;
using ICTDashboard.Auth.Models.Dtos;
using ICTDashboard.Core.Contexts;
using ICTDashboard.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ICTDashboard.Auth.Helpers;

public class FormValidationHelper
{
    public static SignInResult ValidateSignIn(User? user, string password)
    {
        if (user == null || !PasswordHelper.Verify(user.PasswordHash, password))
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

        return new SignInResult { IsSuccess = true };
    }

    public static async Task<(UserRole? ParsedRole, List<ErrorDetail> Errors)>
        ValidateSignUpAsync(IctDbContext db, SignUpRequest r, CancellationToken ct = default)
    {
        var errors = new List<ErrorDetail>();

        var email = r.Email?.Trim();
        var username = r.Username?.Trim();

        if (string.IsNullOrWhiteSpace(r.FirstName))
            errors.Add(new ErrorDetail { Field = "firstName", Message = "First name is required." });
        if (string.IsNullOrWhiteSpace(r.LastName))
            errors.Add(new ErrorDetail { Field = "lastName", Message = "Last name is required." });
        if (string.IsNullOrWhiteSpace(r.Username))
            errors.Add(new ErrorDetail { Field = "username", Message = "Username is required." });
        if (string.IsNullOrWhiteSpace(r.Email))
            errors.Add(new ErrorDetail { Field = "email", Message = "Email is required." });
        if (string.IsNullOrWhiteSpace(r.Password))
            errors.Add(new ErrorDetail { Field = "password", Message = "Password is required." });
        if (string.IsNullOrWhiteSpace(r.ConfirmPassword))
            errors.Add(new ErrorDetail { Field = "confirmPassword", Message = "Confirm password is required." });
        if (r.Password != r.ConfirmPassword)
            errors.Add(new ErrorDetail { Field = "confirmPassword", Message = "Passwords do not match." });

        if (await db.Users.AnyAsync(u => u.Username == username, ct))
            errors.Add(new ErrorDetail { Field = "username", Message = "Username already exists." });
        if (await db.Users.AnyAsync(u => u.Email == email, ct))
            errors.Add(new ErrorDetail { Field = "email", Message = "Email already exists." });

        if (!Enum.TryParse<UserRole>(r.Role, true, out var parsedRole) ||
            (parsedRole != UserRole.Coach && parsedRole != UserRole.Member))
        {
            errors.Add(new ErrorDetail { Field = "role", Message = "Invalid user role." });
            return (null, errors);
        }

        return (parsedRole, errors);
    }
}