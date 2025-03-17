using AutoFixture;
using FluentValidation.TestHelper;
using Workify.Api.Auth.Models.DTOs;

namespace Workify.Api.Auth.UnitTests.Tests.DTOsValidatorsTests.RegisterDtoValidatorTests
{
    public class EmailTests
    {
        private readonly RegisterDtoValidator _validator = new();
        private readonly Fixture _fixture = new();

        public static TheoryData<string> ProperEmails =>
            [
                "aaa@bbb.pl",
                "dasf2f@microsoft.com"
            ];

        [Theory]
        [MemberData(nameof(ProperEmails))]
        public void Should_Allow_Proper_Emails(string email)
        {
            // Arrange
            RegisterDto dto = _fixture.Build<RegisterDto>()
                .With(dto => dto.Email, email)
                .Create();

            // Act
            TestValidationResult<RegisterDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        public static TheoryData<string> WrongEmails =>
            [
                null,
                "",
                "aaaaaaa",
                "aaaa@",
                "@aaaa"
            ];

        [Theory]
        [MemberData(nameof(WrongEmails))]
        public void Should_Throw_When_On_Improper_Emails(string email)
        {
            // Arrange
            RegisterDto dto = _fixture.Build<RegisterDto>()
                .With(dto => dto.Email, email)
                .Create();

            // Act
            TestValidationResult<RegisterDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
    }
}
