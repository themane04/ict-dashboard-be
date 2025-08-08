using Microsoft.AspNetCore.Identity;

namespace ICTDashboard.Auth.Helpers;

public static class AuthPasswordHelper
{
    public static string Hash(string password)
    {
        return new PasswordHasher<string>().HashPassword(null, password);
    }

    public static bool Verify(string hashedPassword, string providedPassword)
    {
        var result = new PasswordHasher<string>().VerifyHashedPassword(null, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}