using Workify.Api.Auth.Models.DTOs;

namespace Workify.Api.Auth.Services
{
    public interface IAuthService
    {
        Task<string> LogInUser(LogInDto dto);
        Task<int> RegisterUser(RegisterDto dto);
    }
}
