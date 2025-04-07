using AutoFixture;
using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Models.Entities;

namespace Workify.Api.Workout.UnitTests.Tests.DTOsFromEntityTests
{
    public class ExerciseDtoTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public void Should_Properly_Map_Predefined_Exercise()
        {
            // Arrange
            PredefinedExercise exercise = _fixture.Create<PredefinedExercise>();

            // Act
            ExerciseDto dto = ExerciseDto.FromEntity(exercise);

            // Assert
            Assert.Equal(exercise.Id, dto.Id);
            Assert.Equal(exercise.Name, dto.Name);
            Assert.Equal(exercise.Description, dto.Description);
            Assert.Equal(exercise.BodyPart, dto.BodyPart);
            Assert.False(dto.IsCustom);
        }

        [Fact]
        public void Should_Properly_Map_User_Exercise()
        {
            // Arrange
            UserExercise exercise = _fixture.Create<UserExercise>();

            // Act
            ExerciseDto dto = ExerciseDto.FromEntity(exercise);

            // Assert
            Assert.Equal(exercise.Id, dto.Id);
            Assert.Equal(exercise.Name, dto.Name);
            Assert.Equal(exercise.Description, dto.Description);
            Assert.Equal(exercise.BodyPart, dto.BodyPart);
            Assert.True(dto.IsCustom);
        }
    }
}
