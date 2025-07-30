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

        public async Task<string> LogInUser(LogInDto dto)
        {
            User? user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Login == dto.Login)
                ?? throw new UnauthorizedAccessException("Wrong login or password.");

            PasswordHasher<User> hasher = new();
            PasswordVerificationResult result = hasher.VerifyHashedPassword(user, user.HashedPassword, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Wrong login or password.");

            return GenerateJwtToken(user.Id);
        }

        public async Task<int> RegisterUser(RegisterDto dto)
        {
            if (await _dbContext.Users.AnyAsync(user => user.Login == dto.Login))
                throw new ArgumentException("User with given login already exists.");
            if (await _dbContext.Users.AnyAsync(user => user.Email == dto.Email))
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

            return newUser.Id;
        }

        private string GenerateJwtToken(int userId)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_config.BearerKey));
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer: CommonConfig.JwtIssuer,
                claims: [new Claim(CommonConfig.JwtClaimUserId, userId.ToString())],
                expires: DateTime.Now.AddHours(8),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
