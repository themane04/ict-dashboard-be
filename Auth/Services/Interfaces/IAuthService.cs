using ICTDashboard.Auth.Models.Dtos;

namespace ICTDashboard.Auth.Services.Interfaces;

public interface IAuthService
{
    Task<SignInResponse> SignInAsync(SignInRequest request);
    Task<SignUpResponse> SignUpAsync(SignUpRequest request);
}