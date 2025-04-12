using AutoFixture;
using FluentValidation.TestHelper;
using Workify.Api.Workout.Models.DTOs.Parameters;
using Workify.Api.Workout.Models.Others;

namespace Workify.Api.Workout.UnitTests.Tests.DTOsValidatorsTests.CreateEditExerciseDtoValidatorTests
{
    public class BodyPartTests
    {
        private readonly CreateEditExerciseDtoValidator _validator = new();
        private readonly Fixture _fixture = new();

        [Fact]
        public void Should_Allow_Proper_Body_Part()
        {
            // Arrange
            CreateEditExerciseDto dto = _fixture.Build<CreateEditExerciseDto>()
                .With(dto => dto.BodyPart, BodyPartEnum.Back)
                .Create();

            // Act
            TestValidationResult<CreateEditExerciseDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.BodyPart);
        }


        [Fact]
        public void Should_Throw_On_Not_Existing_Body_Part()
        {
            // Arrange
            CreateEditExerciseDto dto = _fixture.Build<CreateEditExerciseDto>()
                .With(dto => dto.BodyPart, BodyPartEnum.Back + 999)
                .Create();

            // Act
            TestValidationResult<CreateEditExerciseDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.BodyPart);
        }
    }
}
