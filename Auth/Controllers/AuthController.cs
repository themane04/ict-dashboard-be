using ICTDashboard.Auth.Exceptions;
using ICTDashboard.Auth.Models.Dtos;
using ICTDashboard.Auth.Services.Interfaces;
using ICTDashboard.Core.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ICTDashboard.Auth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("signin")]
    public async Task<IActionResult> Login([FromBody] SignInRequest request)
    {
        try
        {
            var result = await _auth.SignInAsync(request);
            return Ok(result);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ErrorResponse { Errors = ex.Errors });
        }
    }


    [HttpPost("signup")]
    public async Task<IActionResult> Register([FromBody] SignUpRequest request)
    {
        var result = await _auth.SignUpAsync(request);
        return Ok(result);
    }
}