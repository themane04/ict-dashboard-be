namespace ICTDashboard.Auth.Models.Dtos;

public class SignInResponse
{
    public string Token { get; set; } = null!;
    public SignUpResponse User { get; set; } = null!;
}