using System.Net;
using Microsoft.AspNetCore.Mvc;
using Workify.Api.Auth.Models.DTOs;
using Workify.Api.Auth.Services;

namespace Workify.Api.Auth.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LogInDto dto)
    {
        try
        {
            return Ok(await _authService.LogIn(dto));
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<string>> Register([FromBody] RegisterDto dto)
    {
        return StatusCode((int)HttpStatusCode.Created, await _authService.Register(dto));
    }
}
