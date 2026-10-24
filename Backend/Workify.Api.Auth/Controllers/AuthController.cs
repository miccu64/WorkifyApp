using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FluentValidation;
using Workify.Api.Auth.Models.DTOs;
using Workify.Api.Auth.Services;
using Workify.Utils.Communication.Contracts;

namespace Workify.Api.Auth.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService, IPublishEndpoint publishEndpoint) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LogInDto dto, LogInDtoValidator validator, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(dto, ct);

        try
        {
            return Ok(await authService.LogInUser(dto, ct));
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<int>> Register(RegisterDto dto, RegisterDtoValidator validator, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(dto, ct);
        
        int createdUserId = await authService.RegisterUser(dto, ct);

        await publishEndpoint.Publish(new CreatedUserContract(createdUserId), ct);

        return StatusCode((int)HttpStatusCode.Created, createdUserId);
    }
}