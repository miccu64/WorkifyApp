using AutoFixture;
using Microsoft.Extensions.Options;
using Workify.Api.Auth.Database;
using Workify.Api.Auth.Models.DTOs;
using Workify.Api.Auth.Models.Entities;
using Workify.Api.Auth.Services;
using Workify.Api.Auth.UnitTests.Utils;
using Workify.Utils.Config;

namespace Workify.Api.Auth.UnitTests.Tests.AuthServiceTests;

public class RegisterTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task Should_Register_User()
    {
        // Arrange
        using AuthDbContextFactory factory = new();

        IOptions<CommonConfig> config = Options.Create(_fixture.Create<CommonConfig>());
        RegisterDto registerDto = _fixture.Create<RegisterDto>();

        // Act
        using IAuthDbContext authDbContext = await factory.CreateContext();
        AuthService authService = new(authDbContext, config);

        int userId = await authService.Register(registerDto);

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

        IOptions<CommonConfig> config = Options.Create(_fixture.Create<CommonConfig>());

        RegisterDto registerDto = _fixture.Build<RegisterDto>()
            .With(dto => dto.Email, userInDb.Email)
            .Create();

        // Act
        using IAuthDbContext authDbContext = await factory.CreateContext();
        AuthService authService = new(authDbContext, config);

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(() => authService.Register(registerDto));
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

        IOptions<CommonConfig> config = Options.Create(_fixture.Create<CommonConfig>());

        // Act
        using IAuthDbContext authDbContext = await factory.CreateContext();
        AuthService authService = new(authDbContext, config);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => authService.Register(registerDto));
    }
}
