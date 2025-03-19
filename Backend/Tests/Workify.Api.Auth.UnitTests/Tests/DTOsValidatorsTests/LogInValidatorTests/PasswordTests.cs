using AutoFixture;
using FluentValidation.TestHelper;
using Workify.Api.Auth.Models.DTOs;

namespace Workify.Api.Auth.UnitTests.Tests.DTOsValidatorsTests.LoginDtoValidatorTests
{
    public class PasswordTests
    {
        private readonly LogInDtoValidator _validator = new();
        private readonly Fixture _fixture = new();

        public static TheoryData<string> ProperPasswords =>
            [
                new string('a', 8),
                new string('a', 111),
                new string('a', 255),
                "%^&&(*&^$%$-=[];'"
            ];

        [Theory]
        [MemberData(nameof(ProperPasswords))]
        public void Should_Allow(string password)
        {
            // Arrange
            LogInDto dto = _fixture.Build<LogInDto>()
                .With(dto => dto.Password, password)
                .Create();

            // Act
            TestValidationResult<LogInDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        public static TheoryData<string> WrongPasswords =>
            [
                null,
                ""
            ];

        [Theory]
        [MemberData(nameof(WrongPasswords))]
        public void Should_Throw_When_Null_Or_Empty(string password)
        {
            // Arrange
            LogInDto dto = _fixture.Build<LogInDto>()
                .With(dto => dto.Password, password)
                .Create();

            // Act
            TestValidationResult<LogInDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
    }
}
