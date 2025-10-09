using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Workify.Api.Auth.Config;
using Workify.Api.Auth.Database;
using Workify.Api.Auth.Models.DTOs;
using Workify.Api.Auth.Models.Entities;
using Workify.Utils.Config;

namespace Workify.Api.Auth.Services
{
    internal class AuthService(IAuthDbContext dbContext, IOptions<AuthConfig> config) : IAuthService
    {
        public async Task<string> LogInUser(LogInDto dto)
        {
            User user = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Login == dto.Login)
                ?? throw new UnauthorizedAccessException("Wrong login or password.");

            PasswordHasher<User> hasher = new();
            PasswordVerificationResult result = hasher.VerifyHashedPassword(user, user.HashedPassword, dto.Password);

            return result == PasswordVerificationResult.Failed
                ? throw new UnauthorizedAccessException("Wrong login or password.")
                : GenerateJwtToken(user.Id);
        }

        public async Task<int> RegisterUser(RegisterDto dto)
        {
            if (await dbContext.Users.AnyAsync(user => user.Login == dto.Login))
                throw new ArgumentException("User with given login already exists.");
            if (await dbContext.Users.AnyAsync(user => user.Email == dto.Email))
                throw new ArgumentException("User with given email already exists.");

            User newUser = new()
            {
                Login = dto.Login,
                Email = dto.Email,
                HashedPassword = "",
            };
            newUser.HashedPassword = new PasswordHasher<User>().HashPassword(newUser, dto.Password);

            await dbContext.Users.AddAsync(newUser);
            await dbContext.SaveChangesAsync();

            return newUser.Id;
        }

        private string GenerateJwtToken(int userId)
        {
            RSA rsa = RSA.Create();
            rsa.ImportFromPem(
                "-----BEGIN PRIVATE KEY-----\n"
                + config.Value.BearerPrivateKey
                + "\n-----END PRIVATE KEY-----"
            );

            SigningCredentials signingCredentials = new(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            JwtSecurityToken token = new(
                issuer: CommonConfig.JwtIssuer,
                claims: [new Claim(CommonConfig.JwtClaimUserId, userId.ToString())],
                expires: DateTime.Now.AddHours(8),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}