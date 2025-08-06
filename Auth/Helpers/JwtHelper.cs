using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ICTDashboard.Models;
using Microsoft.IdentityModel.Tokens;

namespace ICTDashboard.Auth.Helpers;

public static class JwtHelper
{
    public static string GenerateToken(User user, string jwtKey)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "ictdashboard",
            audience: "ictdashboard",
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}