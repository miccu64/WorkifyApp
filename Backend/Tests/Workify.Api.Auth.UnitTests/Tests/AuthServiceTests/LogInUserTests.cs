using AutoFixture;
using Microsoft.Extensions.Options;
using System.Text;
using Workify.Api.Auth.Database;
using Workify.Api.Auth.Models.DTOs;
using Workify.Api.Auth.Services;
using Workify.Api.Auth.UnitTests.Utils;
using Workify.Utils.Config;

namespace Workify.Api.Auth.UnitTests.Tests.AuthServiceTests
{
    public class LogInUserTests
    {
        private readonly Fixture _fixture;
        private readonly IOptions<CommonConfig> _config;

        public LogInUserTests()
        {
            _fixture = new();
            _config = Options.Create(_fixture.Create<CommonConfig>());
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

            string tokenFirstPart = Encoding.UTF8.GetString(Convert.FromBase64String(jwtToken.Split('.')[0]))!;
            Assert.Contains("HS256", tokenFirstPart);
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
