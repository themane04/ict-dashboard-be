using System.ComponentModel.DataAnnotations;
using ICTDashboard.Auth.Models;

namespace ICTDashboard.Profile.Models;

public class UserProfile
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    [MaxLength(255)] public string? PictureUrl { get; set; }
    public DateOnly? Birthday { get; set; }
    [MaxLength(500)] public string? Bio { get; set; }
}