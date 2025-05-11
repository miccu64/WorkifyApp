using AutoFixture;
using FluentValidation.TestHelper;
using Workify.Api.ExerciseStat.Models.DTOs.Parameters;

namespace Workify.Api.ExerciseStat.UnitTests.Tests.DTOsValidatorsTests.CreateEditStatDtoValidatorTests
{
    public class TimeTests
    {
        private readonly CreateEditStatDtoValidator _validator = new();
        private readonly Fixture _fixture = new();

        public static TheoryData<DateTimeOffset> ProperTimes =>
            [
                new DateTime(2025, 1, 1),
                new DateTime(2111, 1, 3),
                new DateTime(1999, 12, 12)
            ];
        [Theory]
        [MemberData(nameof(ProperTimes))]
        public void Should_Allow(DateTimeOffset time)
        {
            // Arrange
            CreateEditStatDto dto = _fixture.Build<CreateEditStatDto>()
                .With(dto => dto.Time, time)
                .Create();

            // Act
            TestValidationResult<CreateEditStatDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Time);
        }

        [Fact]
        public void Should_Throw_When_Is_Min_Value()
        {
            // Arrange
            CreateEditStatDto dto = _fixture.Build<CreateEditStatDto>()
                .With(dto => dto.Time, DateTimeOffset.MinValue)
                .Create();

            // Act
            TestValidationResult<CreateEditStatDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Time);
        }
    }
}
