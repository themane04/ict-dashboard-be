using System.ComponentModel.DataAnnotations;
using ICTDashboard.Auth.Enums;

namespace ICTDashboard.Auth.Models;

public class User
{
    public int Id { get; set; }
    [MaxLength(50)] public required string Username { get; set; }
    [EmailAddress] [MaxLength(255)] public required string Email { get; set; }
    [MaxLength(100)] public required string PasswordHash { get; set; }
    [MaxLength(100)] public UserRole Role { get; set; }
}