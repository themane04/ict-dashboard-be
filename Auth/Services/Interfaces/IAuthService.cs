using ICTDashboard.Auth.Models.Dtos;

namespace ICTDashboard.Auth.Services.Interfaces;

public interface IAuthService
{
    Task<UserDto?> GetMeAsync(int userId);
    Task<SignInResultDto> SignInAsync(SignInRequestDto requestDto);
    Task SignUpAsync(SignUpRequestDto requestDto);
}