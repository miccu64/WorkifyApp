using AutoFixture;
using Workify.Api.Workout.Database;
using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Models.Entities.Abstractions;
using Workify.Api.Workout.Services;
using Workify.Api.Workout.UnitTests.Utils;

namespace Workify.Api.Workout.UnitTests.Tests.ServicesTests.ExerciseServiceTests
{
    public class GetExercisesTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Return_Empty_Collection()
        {
            // Assert
            using WorkoutDbContextFactory factory = new();

            // Act
            using IWorkoutDbContext dbContext = await factory.CreateContext();
            IEnumerable<ExerciseDto> exercises = await new ExerciseService(dbContext).GetExercises(1);

            // Assert
            Assert.Empty(exercises);
        }

        [Fact]
        public async Task Should_Return_Predefined_And_Current_User_Exercises()
        {
            // Assert
            using WorkoutDbContextFactory factory = new();
            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();

            List<PredefinedExercise> predefinedExercises = _fixture.CreateMany<PredefinedExercise>(5).ToList();
            await arrangeDbContext.PredefinedExercises.AddRangeAsync(predefinedExercises);

            const int userId = 22;
            List<UserExercise> userExercises = _fixture.Build<UserExercise>()
                .With(e => e.UserId, userId)
                .CreateMany(3)
                .ToList();
            await arrangeDbContext.UserExercises.AddRangeAsync(userExercises);

            List<UserExercise> otherUserExercises = _fixture.Build<UserExercise>()
                .With(e => e.UserId, userId + 3)
                .CreateMany(3)
                .ToList();
            await arrangeDbContext.UserExercises.AddRangeAsync(otherUserExercises);

            await arrangeDbContext.SaveChangesAsync();

            // Act
            using IWorkoutDbContext dbContext = await factory.CreateContext();
            IEnumerable<ExerciseDto> exercises = await new ExerciseService(dbContext).GetExercises(userId);

            // Assert
            IEnumerable<int> expectedExercisesIds = predefinedExercises.Select(e => e.Id)
                .Concat(userExercises.Select(e => e.Id))
                .Order();
            IEnumerable<int> responseExercisesIds = exercises.Select(e => e.Id).Order();
            Assert.True(expectedExercisesIds.SequenceEqual(responseExercisesIds));
        }

        [Fact]
        public async Task Should_Return_Correct_Exercises_Data()
        {
            // Assert
            using WorkoutDbContextFactory factory = new();
            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();

            PredefinedExercise predefinedExercise = (await arrangeDbContext.PredefinedExercises.AddAsync(_fixture.Create<PredefinedExercise>())).Entity;

            const int userId = 22;
            UserExercise userExercise = _fixture.Build<UserExercise>()
                .With(e => e.UserId, userId)
                .Create();
            await arrangeDbContext.UserExercises.AddAsync(userExercise);

            await arrangeDbContext.SaveChangesAsync();

            // Act
            using IWorkoutDbContext dbContext = await factory.CreateContext();
            IEnumerable<ExerciseDto> responseExercises = await new ExerciseService(dbContext).GetExercises(userId);

            // Assert
            Assert.Equal(2, responseExercises.Count());

            foreach (Exercise dbExercise in (List<Exercise>)[predefinedExercise, userExercise])
            {
                ExerciseDto dto = responseExercises.Single(e => e.Id == dbExercise.Id);
                Assert.Equal(dto.Name, dbExercise.Name);
                Assert.Equal(dto.Description, dbExercise.Description);
                Assert.Equal(dto.BodyPart, dbExercise.BodyPart);
                Assert.Equal(dto.IsCustom, dbExercise is UserExercise);
            }
        }
    }
}
