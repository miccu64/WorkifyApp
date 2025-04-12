using AutoFixture;
using FluentValidation.TestHelper;
using Workify.Api.Workout.Models.DTOs.Parameters;

namespace Workify.Api.Workout.UnitTests.Tests.DTOsValidatorsTests.CreateEditPlanDtoValidatorTests
{
    public class ExercisesIdsTests
    {
        private readonly CreateEditPlanDtoValidator _validator = new();
        private readonly Fixture _fixture = new();

        [Fact]
        public void Should_Allow_Empty_Array()
        {
            // Arrange
            CreateEditPlanDto dto = _fixture.Build<CreateEditPlanDto>()
                .With(dto => dto.ExercisesIds, [])
                .Create();

            // Act
            TestValidationResult<CreateEditPlanDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ExercisesIds);
        }

        [Fact]
        public void Should_Allow_Ids_Greater_Than_0()
        {
            // Arrange
            CreateEditPlanDto dto = _fixture.Build<CreateEditPlanDto>()
                .With(dto => dto.ExercisesIds, [1, 5, 111, 23978123])
                .Create();

            // Act
            TestValidationResult<CreateEditPlanDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ExercisesIds);
        }

        [Fact]
        public void Should_Throw_When_Exercises_Ids_Is_Null()
        {
            // Arrange
            CreateEditPlanDto dto = _fixture.Build<CreateEditPlanDto>()
                .With(dto => dto.ExercisesIds, (IEnumerable<int>?)null)
                .Create();

            // Act
            TestValidationResult<CreateEditPlanDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ExercisesIds);
        }

        [Fact]
        public void Should_Throw_When_Given_Id_Equal_0()
        {
            // Arrange
            CreateEditPlanDto dto = _fixture.Build<CreateEditPlanDto>()
                .With(dto => dto.ExercisesIds, [0])
                .Create();

            // Act
            TestValidationResult<CreateEditPlanDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ExercisesIds);
        }

        [Fact]
        public void Should_Throw_When_Id_Is_Negative()
        {
            // Arrange
            CreateEditPlanDto dto = _fixture.Build<CreateEditPlanDto>()
                .With(dto => dto.ExercisesIds, [-1, 1])
                .Create();

            // Act
            TestValidationResult<CreateEditPlanDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ExercisesIds);
        }
    }
}
