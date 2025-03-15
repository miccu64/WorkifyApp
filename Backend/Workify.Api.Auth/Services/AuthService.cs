using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Workify.Api.Auth.Database;
using Workify.Api.Auth.Models.DTOs;
using Workify.Api.Auth.Models.Entities;
using Workify.Utils.Config;

namespace Workify.Api.Auth.Services
{
    internal class AuthService(IAuthDbContext dbContext, IOptions<CommonConfig> config) : IAuthService
    {
        private readonly IAuthDbContext _dbContext = dbContext;
        private readonly CommonConfig _config = config.Value;

        public async Task<string> LogIn(LogInDto dto)
        {
            User? user =
                await _dbContext.Users.FirstOrDefaultAsync(user => user.Login == dto.Login)
                ?? throw new UnauthorizedAccessException("Wrong login or password.");

            PasswordHasher<User> hasher = new();
            PasswordVerificationResult result = hasher.VerifyHashedPassword(user, user.HashedPassword, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Wrong login or password.");

            return GenerateJwtToken(user.Id);
        }

        public async Task<string> Register(RegisterDto dto)
        {
            if (_dbContext.Users.Any(user => user.Login == dto.Login))
                throw new ArgumentException("User with given login already exists.");
            if (_dbContext.Users.Any(user => user.Email == dto.Email))
                throw new ArgumentException("User with given email already exists.");

            User newUser = new()
            {
                Login = dto.Login,
                Email = dto.Email,
                HashedPassword = "",
            };
            newUser.HashedPassword = new PasswordHasher<User>().HashPassword(newUser, dto.Password);

            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();

            return GenerateJwtToken(newUser.Id);
        }

        private string GenerateJwtToken(int userId)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_config.BearerKey));
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials,
                issuer: _config.JwtIssuer,
                claims: [new Claim(_config.JwtClaimUserId, userId.ToString())]
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
