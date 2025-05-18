using AutoFixture;
using Microsoft.Extensions.Options;
using Workify.Api.Auth.Database;
using Workify.Api.Auth.Models.DTOs;
using Workify.Api.Auth.Models.Entities;
using Workify.Api.Auth.Services;
using Workify.Api.Auth.UnitTests.Utils;
using Workify.Utils.Config;

namespace Workify.Api.Auth.UnitTests.Tests.AuthServiceTests;

public class RegisterUserTests
{
    private readonly Fixture _fixture;
    private readonly IOptions<CommonConfig> _config;

    public RegisterUserTests()
    {
        _fixture = new();
        _config = Options.Create(_fixture.Create<CommonConfig>());
    }

    [Fact]
    public async Task Should_Register_User()
    {
        // Arrange
        using AuthDbContextFactory factory = new();

        RegisterDto registerDto = _fixture.Create<RegisterDto>();

        // Act
        using IAuthDbContext authDbContext = await factory.CreateContext();
        AuthService authService = new(authDbContext, _config);

        int userId = await authService.RegisterUser(registerDto);

        // Assert
        using IAuthDbContext assertDbContext = await factory.CreateContext();
        User? dbUser = await assertDbContext.Users.FindAsync(userId);

        Assert.NotNull(dbUser);
        Assert.Equal(registerDto.Login, dbUser.Login);
        Assert.Equal(registerDto.Email, dbUser.Email);
        Assert.NotEqual(registerDto.Password, dbUser.HashedPassword);
    }

    [Fact]
    public async Task Should_Throw_When_User_With_Given_Email_Exists()
    {
        // Arrange
        using AuthDbContextFactory factory = new();

        User userInDb = _fixture.Create<User>();

        using IAuthDbContext arrangeDbContext = await factory.CreateContext();
        await arrangeDbContext.Users.AddAsync(userInDb);
        await arrangeDbContext.SaveChangesAsync();

        RegisterDto registerDto = _fixture.Build<RegisterDto>()
            .With(dto => dto.Email, userInDb.Email)
            .Create();

        // Act
        using IAuthDbContext authDbContext = await factory.CreateContext();
        AuthService authService = new(authDbContext, _config);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(() => authService.RegisterUser(registerDto));
    }

    [Fact]
    public async Task Should_Throw_When_User_With_Given_Login_Exists()
    {
        // Arrange
        using AuthDbContextFactory factory = new();

        User userInDb = _fixture.Create<User>();

        using IAuthDbContext arrangeDbContext = await factory.CreateContext();
        await arrangeDbContext.Users.AddAsync(userInDb);
        await arrangeDbContext.SaveChangesAsync();

        RegisterDto registerDto = _fixture.Build<RegisterDto>()
            .With(dto => dto.Email, userInDb.Email)
            .Create();

        // Act
        using IAuthDbContext authDbContext = await factory.CreateContext();
        AuthService authService = new(authDbContext, _config);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => authService.RegisterUser(registerDto));
    }
}
