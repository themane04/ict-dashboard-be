using System.ComponentModel.DataAnnotations;
using ICTDashboard.Auth.Enums;
using ICTDashboard.Profile.Models;

namespace ICTDashboard.Auth.Models;

public class User
{
    public int Id { get; set; }

    [MaxLength(50)] public required string Username { get; set; }
    [EmailAddress, MaxLength(255)] public required string Email { get; set; }

    [MaxLength(255)] public required string PasswordHash { get; set; }

    public AuthUserRole Role { get; set; }

    [MaxLength(50)] public required string FirstName { get; set; }
    [MaxLength(50)] public required string LastName { get; set; }

    public required UserProfile Profile { get; set; }
}