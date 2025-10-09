using System.Security.Cryptography;
using AutoFixture;
using Microsoft.Extensions.Options;
using System.Text;
using Workify.Api.Auth.Config;
using Workify.Api.Auth.Database;
using Workify.Api.Auth.Models.DTOs;
using Workify.Api.Auth.Services;
using Workify.Api.Auth.UnitTests.Utils;

namespace Workify.Api.Auth.UnitTests.Tests.AuthServiceTests
{
    public class LogInUserTests
    {
        private readonly Fixture _fixture;
        private readonly IOptions<AuthConfig> _config;

        public LogInUserTests()
        {
            _fixture = new();

            using var rsa = RSA.Create(2048);

            string privateKeyBase64 = Convert.ToBase64String(rsa.ExportPkcs8PrivateKey());
            string publicKeyBase64 = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());

            _config = Options.Create(
                _fixture.Build<AuthConfig>()
                    .With(c => c.BearerPrivateKey, privateKeyBase64)
                    .With(c => c.BearerPublicKey, publicKeyBase64)
                    .Create()
            );
        }

        [Fact]
        public async Task Should_Return_Jwt_Token_After_Login()
        {
            // Arrange
            using AuthDbContextFactory factory = new();

            RegisterDto registerDto = await RegisterUserViaService(factory);

            using IAuthDbContext arrangeDbContext = await factory.CreateContext();
            AuthService authService = new(arrangeDbContext, _config);

            LogInDto dto = new(registerDto.Login, registerDto.Password);

            // Act
            string jwtToken = await authService.LogInUser(dto);

            // Assert
            Assert.NotEmpty(jwtToken);
            Assert.Contains(".", jwtToken);

            string tokenFirstPart = Encoding.UTF8.GetString(Convert.FromBase64String(jwtToken.Split('.')[0]));
            Assert.Contains("RS256", tokenFirstPart);
            Assert.Contains("JWT", tokenFirstPart);
        }

        [Fact]
        public async Task Should_Throw_When_Login_And_Or_Password_Exists_But_Are_Wrong()
        {
            // Arrange
            using AuthDbContextFactory factory = new();

            RegisterDto registerDto1 = await RegisterUserViaService(factory);
            RegisterDto registerDto2 = await RegisterUserViaService(factory);

            using IAuthDbContext authDbContext = await factory.CreateContext();
            AuthService authService = new(authDbContext, _config);

            LogInDto wrongLogInDto = new(registerDto1.Login, registerDto2.Password);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => authService.LogInUser(wrongLogInDto));
        }

        [Fact]
        public async Task Should_Throw_When_Login_Not_Exists()
        {
            // Arrange
            using AuthDbContextFactory factory = new();

            RegisterDto _ = await RegisterUserViaService(factory);

            using IAuthDbContext authDbContext = await factory.CreateContext();
            AuthService authService = new(authDbContext, _config);

            LogInDto dto = _fixture.Create<LogInDto>();

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => authService.LogInUser(dto));
        }

        private async Task<RegisterDto> RegisterUserViaService(AuthDbContextFactory factory)
        {
            using IAuthDbContext arrangeDbContext = await factory.CreateContext();
            AuthService authService = new(arrangeDbContext, _config);

            RegisterDto registerDto = _fixture.Create<RegisterDto>();
            int userId = await authService.RegisterUser(registerDto);

            Assert.True(userId > 0);

            return registerDto;
        }
    }
}