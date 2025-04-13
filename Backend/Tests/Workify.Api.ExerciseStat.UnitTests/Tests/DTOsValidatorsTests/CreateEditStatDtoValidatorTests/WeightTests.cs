using AutoFixture;
using FluentValidation.TestHelper;
using Workify.Api.ExerciseStat.Models.DTOs.Parameters;

namespace Workify.Api.ExerciseStat.UnitTests.Tests.DTOsValidatorsTests.CreateEditStatDtoValidatorTests
{
    public class WeightTests
    {
        private readonly CreateEditStatDtoValidator _validator = new();
        private readonly Fixture _fixture = new();

        [Theory]
        [InlineData(0)]
        [InlineData(0.0001)]
        [InlineData(1)]
        [InlineData(111.11)]
        [InlineData(3333)]
        public void Should_Allow_Greater_Or_Equal_Zero(double weight)
        {
            // Arrange
            CreateEditStatDto dto = _fixture.Build<CreateEditStatDto>()
                .With(dto => dto.Weight, weight)
                .Create();

            // Act
            TestValidationResult<CreateEditStatDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Weight);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-0.01)]
        [InlineData(-333)]
        [InlineData(-333.33)]
        public void Should_Throw_On_Lower_Than_Zero(double weight)
        {
            // Arrange
            CreateEditStatDto dto = _fixture.Build<CreateEditStatDto>()
                .With(dto => dto.Weight, weight)
                .Create();

            // Act
            TestValidationResult<CreateEditStatDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Weight);
        }
    }
}
