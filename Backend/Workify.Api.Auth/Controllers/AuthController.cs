using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Workify.Api.Auth.Models.DTOs;
using Workify.Api.Auth.Services;
using Workify.Utils.Communication.Contracts;

namespace Workify.Api.Auth.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService, IPublishEndpoint publishEndpoint) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LogInDto dto)
    {
        try
        {
            return Ok(await _authService.LogInUser(dto));
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<int>> Register(RegisterDto dto)
    {
        int createdUserId = await _authService.RegisterUser(dto);

        await _publishEndpoint.Publish(new CreatedUserContract(createdUserId));

        return StatusCode((int)HttpStatusCode.Created, createdUserId);
    }
}
