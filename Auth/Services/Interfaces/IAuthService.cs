using ICTDashboard.Auth.Models.Dtos;

namespace ICTDashboard.Auth.Services.Interfaces;

public interface IAuthService
{
    Task<SignInResult> SignInAsync(SignInRequest request);
    Task<SignUpResponse> SignUpAsync(SignUpRequest request);
}