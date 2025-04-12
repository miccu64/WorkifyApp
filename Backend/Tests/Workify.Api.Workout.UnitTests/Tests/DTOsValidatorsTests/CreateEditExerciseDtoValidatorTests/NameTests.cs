using AutoFixture;
using FluentValidation.TestHelper;
using Workify.Api.Workout.Models.DTOs.Parameters;

namespace Workify.Api.Workout.UnitTests.Tests.DTOsValidatorsTests.CreateEditExerciseDtoValidatorTests
{
    public class NameTests
    {
        private readonly CreateEditExerciseDtoValidator _validator = new();
        private readonly Fixture _fixture = new();

        public static TheoryData<string> ProperNames =>
            [
                "a",
                new string('a', 255),
                new string('a', 21)
            ];

        [Theory]
        [MemberData(nameof(ProperNames))]
        public void Should_Allow_Proper_Names(string name)
        {
            // Arrange
            CreateEditExerciseDto dto = _fixture.Build<CreateEditExerciseDto>()
                .With(dto => dto.Name, name)
                .Create();

            // Act
            TestValidationResult<CreateEditExerciseDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        public static TheoryData<string> WrongNames =>
            [
                null,
                "",
                new string('a', 256),
                new string('a', 344)
            ];

        [Theory]
        [MemberData(nameof(WrongNames))]
        public void Should_Throw_When_Null_Or_Empty_Or_Too_Long(string name)
        {
            // Arrange
            CreateEditExerciseDto dto = _fixture.Build<CreateEditExerciseDto>()
                .With(dto => dto.Name, name)
                .Create();

            // Act
            TestValidationResult<CreateEditExerciseDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }
    }
}
