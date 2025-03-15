using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Workify.Api.Auth.Config;
using Workify.Api.Auth.Database;
using Workify.Api.Auth.Models.DTOs;
using Workify.Api.Auth.Models.Entities;

namespace Workify.Api.Auth.Services
{
    internal class AuthService(AuthDbContext dbContext, AuthConfig config) : IAuthService
    {
        private readonly AuthDbContext _dbContext = dbContext;
        private readonly AuthConfig _config = config;

        public string LogIn(LogInDto dto) { }

        public async Task<string> Register(RegisterDto dto)
        {
            User? existingUser = _dbContext.Users.FirstOrDefault(user =>
                user.Login == dto.Login || user.Email == dto.Email
            );

            if (existingUser?.Login == dto.Login)
                throw new ArgumentException("User with given login already exists.");
            if (existingUser?.Email == dto.Email)
                throw new ArgumentException("User with given email already exists.");

            User newUser = new()
            {
                Login = dto.Login,
                Email = dto.Email,
                HashedPassword = "",
            };
            newUser.HashedPassword = new PasswordHasher<User>().HashPassword(newUser, dto.Password);

            await _dbContext.AddAsync(newUser);
        }

        private string GenerateJwtToken()
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_config.BearerKey));
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer: "Workify",
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
