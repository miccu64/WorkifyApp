using AutoFixture;
using FluentValidation.TestHelper;
using Workify.Api.Auth.Models.DTOs;

namespace Workify.Api.Auth.UnitTests.Tests.DTOsValidatorsTests.LoginDtoValidatorTests
{
    public class LoginTests
    {
        private readonly LogInDtoValidator _validator = new();
        private readonly Fixture _fixture = new();

        public static TheoryData<string> ProperLogins =>
            [
                new string('a', 4),
                new string('a', 31),
                new string('a', 21)
            ];

        [Theory]
        [MemberData(nameof(ProperLogins))]
        public void Should_Allow(string login)
        {
            // Arrange
            LogInDto dto = _fixture.Build<LogInDto>()
                .With(dto => dto.Login, login)
                .Create();

            // Act
            TestValidationResult<LogInDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Login);
        }

        public static TheoryData<string> WrongLogins =>
            [
                null,
                ""
            ];

        [Theory]
        [MemberData(nameof(WrongLogins))]
        public void Should_Throw_When_Null_Or_Empty(string login)
        {
            // Arrange
            LogInDto dto = _fixture.Build<LogInDto>()
                .With(dto => dto.Login, login)
                .Create();

            // Act
            TestValidationResult<LogInDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Login);
        }
    }
}
