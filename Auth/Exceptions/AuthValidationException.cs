using ICTDashboard.Core.Models;

namespace ICTDashboard.Auth.Exceptions;

public class ValidationException : Exception
{
    public List<ErrorDetail> Errors { get; }

    public ValidationException(List<ErrorDetail> errors)
    {
        Errors = errors;
    }
}