namespace ICTDashboard.Auth.Models.Dtos;

public class SignInRequestDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}