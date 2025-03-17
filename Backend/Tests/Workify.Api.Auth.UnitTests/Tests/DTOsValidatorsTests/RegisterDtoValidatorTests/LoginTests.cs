using AutoFixture;
using FluentValidation.TestHelper;
using Workify.Api.Auth.Models.DTOs;

namespace Workify.Api.Auth.UnitTests.Tests.DTOsValidatorsTests.RegisterDtoValidatorTests
{
    public class LoginTests
    {
        private readonly RegisterDtoValidator _validator = new();
        private readonly Fixture _fixture = new();

        public static TheoryData<string> ProperLogins =>
            [
                new string('a', 4),
                new string('a', 32),
                new string('a', 21)
            ];

        [Theory]
        [MemberData(nameof(ProperLogins))]
        public void Should_Allow_4_To_32_Length(string login)
        {
            // Arrange
            RegisterDto dto = _fixture.Build<RegisterDto>()
                .With(dto => dto.Login, login)
                .Create();

            // Act
            TestValidationResult<RegisterDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Login);
        }

        public static TheoryData<string> WrongLogins =>
            [
                null,
                "",
                new string('a', 3),
                new string('a', 33)
            ];

        [Theory]
        [MemberData(nameof(WrongLogins))]
        public void Should_Throw_When_Out_Of_4_To_32_Length(string login)
        {
            // Arrange
            RegisterDto dto = _fixture.Build<RegisterDto>()
                .With(dto => dto.Login, login)
                .Create();

            // Act
            TestValidationResult<RegisterDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Login);
        }
    }
}
