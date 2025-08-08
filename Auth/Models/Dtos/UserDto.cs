using ICTDashboard.Profile.Models.Dtos;

namespace ICTDashboard.Auth.Models.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public UserProfileDto Profile { get; set; } = new();
}