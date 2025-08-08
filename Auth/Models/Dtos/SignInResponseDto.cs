namespace ICTDashboard.Auth.Models.Dtos;

public class SignInResponseDto
{
    public string Token { get; set; } = null!;
    public SignUpResponseDto User { get; set; } = null!;
}