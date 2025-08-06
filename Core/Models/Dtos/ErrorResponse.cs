namespace ICTDashboard.Core.Models.Dtos;

public class ErrorResponse
{
    public List<ErrorDetail> Errors { get; set; } = new();
}