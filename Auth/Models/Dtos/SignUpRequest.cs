using ICTDashboard.Auth.Enums;

namespace ICTDashboard.Auth.Models.Dtos;

public class SignUpRequest
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
    public required UserRole Role { get; set; } = UserRole.Member;
}