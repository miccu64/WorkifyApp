using AutoFixture;
using FluentValidation.TestHelper;
using Workify.Api.Auth.Models.DTOs;

namespace Workify.Api.Auth.UnitTests.Tests.DTOsValidatorsTests.RegisterDtoValidatorTests
{
    public class PasswordTests
    {
        private readonly RegisterDtoValidator _validator = new();
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
        public void Should_Allow_4_To_255_Length(string password)
        {
            // Arrange
            RegisterDto dto = _fixture.Build<RegisterDto>()
                .With(dto => dto.Password, password)
                .Create();

            // Act
            TestValidationResult<RegisterDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        public static TheoryData<string> WrongPasswords =>
            [
                null,
                "",
                new string('a', 7),
                new string('a', 256)
            ];

        [Theory]
        [MemberData(nameof(WrongPasswords))]
        public void Should_Throw_When_Out_Of_8_To_255_Length(string password)
        {
            // Arrange
            RegisterDto dto = _fixture.Build<RegisterDto>()
                .With(dto => dto.Password, password)
                .Create();

            // Act
            TestValidationResult<RegisterDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }
    }
}
