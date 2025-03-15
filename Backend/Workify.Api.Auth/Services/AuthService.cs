using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Workify.Api.Auth.Services
{
    internal class AuthService : IAuthService
    {
        public AuthService(){

        }

        public string GenerateJwtToken(string username)
        {
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes("your_secret_key"));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "Workify",
                claims: [],
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
