using ICTDashboard.Core.Models;

namespace ICTDashboard.Auth.Models.Dtos;

public class SignInResultDto
{
    public bool IsSuccess { get; set; }
    public string? Token { get; set; }
    public ErrorDetail[]? Errors { get; set; }
}