using Workify.Api.Auth.Models.DTOs;

namespace Workify.Api.Auth.Services
{
    public interface IAuthService
    {
        Task<string> LogInUser(LogInDto dto, CancellationToken ct);
        Task<int> RegisterUser(RegisterDto dto, CancellationToken ct);
    }
}
