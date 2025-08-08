namespace ICTDashboard.Profile.Models.Dtos;

public class UserProfileDto
{
    public string? PictureUrl { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? Bio { get; set; }
}