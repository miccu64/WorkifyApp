using Microsoft.AspNetCore.Mvc;
using Workify.Api.Auth.Models.DTOs;

namespace Workify.Api.Auth.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto login)
    {
        return Unauthorized();
    }
}
