using AutoFixture;
using Workify.Api.Workout.Database;
using Workify.Api.Workout.Models.DTOs.Parameters;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Services;
using Workify.Api.Workout.UnitTests.Utils;

namespace Workify.Api.Workout.UnitTests.Tests.ServicesTests.ExerciseServiceTests
{
    public class CreateExerciseTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Create_Exercise()
        {
            // Arrange
            using DbContextFactory factory = new();
            CreateEditExerciseDto dto = _fixture.Create<CreateEditExerciseDto>();
            const int userId = 44;

            // Act
            using IWorkoutDbContext dbContext = await factory.CreateContext();
            ExerciseService exerciseService = new(dbContext);
            int exerciseId = await exerciseService.CreateExercise(userId, dto);

            // Assert
            Assert.True(exerciseId > 0);

            using IWorkoutDbContext assertDbContext = await factory.CreateContext();
            UserExercise? userExercise = await assertDbContext.UserExercises.FindAsync(exerciseId);

            Assert.NotNull(userExercise);
            Assert.Equal(userId, userExercise.UserId);
            Assert.Equal(dto.Name, userExercise.Name);
            Assert.Equal(dto.Description, userExercise.Description);
            Assert.Equal(dto.BodyPart, userExercise.BodyPart);
        }

        [Fact]
        public async Task Should_Create_Two_Exercises_With_Same_Data()
        {
            // Arrange
            using DbContextFactory factory = new();
            CreateEditExerciseDto dto = _fixture.Create<CreateEditExerciseDto>();
            const int userId = 44;

            // Act
            using IWorkoutDbContext dbContext = await factory.CreateContext();
            ExerciseService exerciseService = new(dbContext);
            int exerciseId1 = await exerciseService.CreateExercise(userId, dto);
            int exerciseId2 = await exerciseService.CreateExercise(userId, dto);

            // Assert
            Assert.True(exerciseId1 > 0);
            Assert.True(exerciseId2 > 0);
            Assert.NotEqual(exerciseId1, exerciseId2);
        }
    }
}
