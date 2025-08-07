using System.Net;
using System.Text.Json;
using ICTDashboard.Auth.Exceptions;
using ICTDashboard.Core.Models;
using ICTDashboard.Core.Models.Dtos;

namespace ICTDashboard.Core.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validation error: {Errors}", ex.Errors);
            await WriteErrorResponse(context, HttpStatusCode.BadRequest, new ErrorResponse { Errors = ex.Errors });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");
            await WriteErrorResponse(context, HttpStatusCode.InternalServerError, new ErrorResponse
            {
                Errors = new List<ErrorDetail>
                {
                    new() { Field = "unknown", Message = "Something went wrong." }
                }
            });
        }
    }

    private async Task WriteErrorResponse(HttpContext context, HttpStatusCode statusCode, object response)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}