using Workify.Api.Auth.Models.DTOs;

namespace Workify.Api.Auth.Services
{
    public interface IAuthService
    {
        public Task<string> LogIn(LogInDto dto);
        public Task<int> Register(RegisterDto dto);
    }
}
