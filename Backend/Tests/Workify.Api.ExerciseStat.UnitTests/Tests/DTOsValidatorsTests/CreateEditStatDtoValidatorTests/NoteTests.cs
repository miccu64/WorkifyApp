using AutoFixture;
using FluentValidation.TestHelper;
using Workify.Api.ExerciseStat.Models.DTOs.Parameters;

namespace Workify.Api.ExerciseStat.UnitTests.Tests.DTOsValidatorsTests.CreateEditStatDtoValidatorTests
{
    public class NoteTests
    {
        private readonly CreateEditStatDtoValidator _validator = new();
        private readonly Fixture _fixture = new();

        public static TheoryData<string> ProperNotes =>
            [
                null,
                "",
                new string('a', 1),
                new string('a', 31),
                new string('a', 111)
            ];

        [Theory]
        [MemberData(nameof(ProperNotes))]
        public void Should_Allow(string note)
        {
            // Arrange
            CreateEditStatDto dto = _fixture.Build<CreateEditStatDto>()
                .With(dto => dto.Note, note)
                .Create();

            // Act
            TestValidationResult<CreateEditStatDto> result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Note);
        }
    }
}
