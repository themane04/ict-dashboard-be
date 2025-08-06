namespace ICTDashboard.Auth.Models.Dtos;

public class SignInRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}