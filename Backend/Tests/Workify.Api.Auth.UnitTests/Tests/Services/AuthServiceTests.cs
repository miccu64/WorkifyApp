using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using Workify.Api.Auth.Database;
using Workify.Api.Auth.Models.DTOs;
using Workify.Api.Auth.Services;
using Workify.Utils.Config;

namespace Workify.Api.Auth.UnitTests.Tests.Services;

public class AuthServiceTests
{
    [Fact]
    public async Task Register_Should_Register_User()
    {
        // Arrange
        IAuthDbContext dbContext = new Mock<IAuthDbContext>().Object;
        IOptions<CommonConfig> config = new Mock<IOptions<CommonConfig>>().Object;
        AuthService authService = new(dbContext, config);

        Fixture fixture = new();
        fixture.Customize<RegisterDto>(c => c.With(dto => dto.Email, "mail@mail.pl"));
        RegisterDto registerDto = fixture.Create<RegisterDto>();

        // Act
        string token = await authService.Register(registerDto);

        // Assert
        Assert.NotEmpty(token);
    }
}
