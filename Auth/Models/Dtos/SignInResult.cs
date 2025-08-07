using ICTDashboard.Core.Models;

namespace ICTDashboard.Auth.Models.Dtos;

public class SignInResult
{
    public bool IsSuccess { get; set; }
    public string? Token { get; set; }
    public SignUpResponse? User { get; set; }
    public ErrorDetail[]? Errors { get; set; }
}