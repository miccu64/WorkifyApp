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
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task Should_Register_User()
    {
        // Arrange
        Mock<IAuthDbContext> mockDbContext = AuthDbContextMock.GetMockDbContext();
        IOptions<CommonConfig> config = Options.Create(_fixture.Create<CommonConfig>());
        AuthService authService = new(mockDbContext.Object, config);

        RegisterDto registerDto = _fixture.Create<RegisterDto>();

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
        User userInDb = _fixture.Create<User>();

        Mock<IAuthDbContext> mockDbContext = AuthDbContextMock.GetMockDbContext([userInDb]);

        IOptions<CommonConfig> config = Options.Create(_fixture.Create<CommonConfig>());
        AuthService authService = new(mockDbContext.Object, config);

        RegisterDto registerDto = _fixture.Build<RegisterDto>()
            .With(dto => dto.Email, userInDb.Email)
            .Create();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => authService.Register(registerDto));
    }

    [Fact]
    public async Task Should_Throw_When_User_With_Given_Login_Exists()
    {
        // Arrange
        User userInDb = _fixture.Create<User>();

        Mock<IAuthDbContext> mockDbContext = AuthDbContextMock.GetMockDbContext([userInDb]);

        IOptions<CommonConfig> config = Options.Create(_fixture.Create<CommonConfig>());
        AuthService authService = new(mockDbContext.Object, config);

        RegisterDto registerDto = _fixture.Build<RegisterDto>()
            .With(dto => dto.Email, userInDb.Email)
            .Create();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => authService.Register(registerDto));
    }
}
