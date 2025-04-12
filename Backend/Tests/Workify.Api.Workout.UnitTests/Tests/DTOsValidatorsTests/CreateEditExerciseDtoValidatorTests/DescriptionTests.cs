using AutoFixture;
using FluentValidation.TestHelper;
using Workify.Api.Workout.Models.DTOs.Parameters;

namespace Workify.Api.Workout.UnitTests.Tests.DTOsValidatorsTests.CreateEditExerciseDtoValidatorTests
{
    public class DescriptionTests
    {
        private readonly CreateEditExerciseDtoValidator _validator = new();
        private readonly Fixture _fixture = new();

        public static TheoryData<string> ProperDescriptions =>
            [
                null,
                "",
                "a",
                new string('a', 1023),
                new string('a', 21)
            ];

        [Theory]
        [MemberData(nameof(ProperDescriptions))]
        public void Should_Allow_Proper_Descriptions(string description)
        {
            // Arrange
            CreateEditExerciseDto dto = _fixture.Build<CreateEditExerciseDto>()
                .With(dto => dto.Description, description)
                .Create();

            // Act
            TestValidationResult<CreateEditExerciseDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        public static TheoryData<string> WrongDescriptions =>
            [
                new string('a', 1024),
                new string('a', 1111)
            ];

        [Theory]
        [MemberData(nameof(WrongDescriptions))]
        public void Should_Throw_When_Too_Long(string description)
        {
            // Arrange
            CreateEditExerciseDto dto = _fixture.Build<CreateEditExerciseDto>()
                .With(dto => dto.Description, description)
                .Create();

            // Act
            TestValidationResult<CreateEditExerciseDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }
    }
}
