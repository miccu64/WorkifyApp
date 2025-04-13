using AutoFixture;
using FluentValidation.TestHelper;
using Workify.Api.ExerciseStat.Models.DTOs.Parameters;

namespace Workify.Api.ExerciseStat.UnitTests.Tests.DTOsValidatorsTests.CreateEditStatDtoValidatorTests
{
    public class RepsTests
    {
        private readonly CreateEditStatDtoValidator _validator = new();
        private readonly Fixture _fixture = new();

        [Theory]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(33)]
        public void Should_Allow_Greater_Than_Zero(int reps)
        {
            // Arrange
            CreateEditStatDto dto = _fixture.Build<CreateEditStatDto>()
                .With(dto => dto.Reps, reps)
                .Create();

            // Act
            TestValidationResult<CreateEditStatDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Reps);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-33)]
        public void Should_Throw_On_Lower_Than_Zero(int reps)
        {
            // Arrange
            CreateEditStatDto dto = _fixture.Build<CreateEditStatDto>()
                .With(dto => dto.Reps, reps)
                .Create();

            // Act
            TestValidationResult<CreateEditStatDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Reps);
        }
    }
}
