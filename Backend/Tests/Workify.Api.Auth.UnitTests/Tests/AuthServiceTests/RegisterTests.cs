using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using Workify.Api.Auth.Database;
using Workify.Api.Auth.Models.DTOs;
using Workify.Api.Auth.Models.Entities;
using Workify.Api.Auth.Services;
using Workify.Api.Auth.UnitTests.Mocks;
using Workify.Utils.Config;

namespace Workify.Api.Auth.UnitTests.Tests.AuthServiceTests;

public class RegisterTests
{
    [Fact]
    public async Task Should_Register_User()
    {
        // Arrange
        Fixture fixture = new();

        Mock<IAuthDbContext> mockDbContext = AuthDbContextMock.GetMockDbContext();
        IOptions<CommonConfig> config = Options.Create(fixture.Create<CommonConfig>());
        AuthService authService = new(mockDbContext.Object, config);

        fixture.Customize<RegisterDto>(c => c.With(dto => dto.Email, "mail@mail.pl"));
        RegisterDto registerDto = fixture.Create<RegisterDto>();

        // Act
        int userId = await authService.Register(registerDto);

        // Assert
        mockDbContext.Verify(db => db.Users.AddAsync(It.Is<User>(user =>
           user.Id == userId && user.Login == registerDto.Login && user.Email == registerDto.Email && user.HashedPassword != registerDto.Password
        ), It.IsAny<CancellationToken>()), Times.Once);
        mockDbContext.Verify(db => db.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Should_Throw_When_User_With_Given_Email_Exists()
    {
        // Arrange
        Fixture fixture = new();

        fixture.Customize<User>(c => c.With(user => user.Email, "mail@mail.pl"));
        User userInDb = fixture.Create<User>();
        Mock<IAuthDbContext> mockDbContext = AuthDbContextMock.GetMockDbContext([userInDb]);

        IOptions<CommonConfig> config = Options.Create(fixture.Create<CommonConfig>());
        AuthService authService = new(mockDbContext.Object, config);

        fixture.Customize<RegisterDto>(c => c.With(dto => dto.Email, userInDb.Email));
        RegisterDto registerDto = fixture.Create<RegisterDto>();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => authService.Register(registerDto));
    }

    [Fact]
    public async Task Should_Throw_When_User_With_Given_Login_Exists()
    {
        // Arrange
        Fixture fixture = new();

        fixture.Customize<User>(c => c.With(user => user.Email, "mail@mail.pl"));
        User userInDb = fixture.Create<User>();
        Mock<IAuthDbContext> mockDbContext = AuthDbContextMock.GetMockDbContext([userInDb]);

        IOptions<CommonConfig> config = Options.Create(fixture.Create<CommonConfig>());
        AuthService authService = new(mockDbContext.Object, config);

        fixture.Customize<RegisterDto>(c => c
            .With(dto => dto.Email, "e12@erf.pl")
            .With(user => user.Login, userInDb.Login)
        );
        RegisterDto registerDto = fixture.Create<RegisterDto>();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => authService.Register(registerDto));
    }

    [Theory]
    [InlineData("")]
    [InlineData("test")]
    [InlineData("test@t@t")]
    public async Task Should_Throw_On_Invalid_Email(string email)
    {
        // Arrange
        Mock<IAuthDbContext> mockDbContext = AuthDbContextMock.GetMockDbContext();
        Fixture fixture = new();

        IOptions<CommonConfig> config = Options.Create(fixture.Create<CommonConfig>());
        AuthService authService = new(mockDbContext.Object, config);

        fixture.Customize<RegisterDto>(c => c.With(dto => dto.Email, email));
        RegisterDto registerDto = fixture.Create<RegisterDto>();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => authService.Register(registerDto));
    }
}
