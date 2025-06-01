using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Workify.Api.Workout.Database;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Services;
using Workify.Api.Workout.UnitTests.Utils;

namespace Workify.Api.Workout.UnitTests.Tests.ServicesTests.ExerciseServiceTests
{
    public class DeleteExerciseTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Delete_User_Exercise_Not_Assigned_To_Plan()
        {
            // Arrange
            using DbContextFactory factory = new();

            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();
            UserExercise exercise = (await arrangeDbContext.UserExercises.AddAsync(_fixture.Create<UserExercise>())).Entity;
            await arrangeDbContext.SaveChangesAsync();

            // Act
            using IWorkoutDbContext dbContext = await factory.CreateContext();
            int deletedExerciseId = await new ExerciseService(dbContext).DeleteExercise(exercise.Id, exercise.UserId);

            // Assert
            Assert.Equal(exercise.Id, deletedExerciseId);

            using IWorkoutDbContext assertDbContext = await factory.CreateContext();
            Assert.Null(await assertDbContext.UserExercises.FindAsync(deletedExerciseId));
        }

        [Fact]
        public async Task Should_Delete_User_Exercise_With_Removal_From_Plan()
        {
            // Arrange
            using DbContextFactory factory = new();
            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();

            UserExercise exercise1 = _fixture.Create<UserExercise>();
            int userId = exercise1.UserId;
            UserExercise exercise2 = _fixture.Build<UserExercise>()
                .With(e => e.UserId, userId)
                .Create();
            await arrangeDbContext.UserExercises.AddRangeAsync([exercise1, exercise2]);

            UserPlan plan = _fixture.Build<UserPlan>()
                .With(p => p.Exercises, [exercise1, exercise2])
                .With(p => p.UserId, userId)
                .Create();
            await arrangeDbContext.UserPlans.AddAsync(plan);

            await arrangeDbContext.SaveChangesAsync();

            // Act
            using IWorkoutDbContext dbContext = await factory.CreateContext();
            int deletedExerciseId = await new ExerciseService(dbContext).DeleteExercise(exercise1.Id, userId);

            // Assert
            Assert.Equal(exercise1.Id, deletedExerciseId);

            using IWorkoutDbContext assertDbContext = await factory.CreateContext();
            Assert.Null(await assertDbContext.UserExercises.FindAsync(deletedExerciseId));

            UserPlan planAfterDelete = await assertDbContext.UserPlans.Include(p => p.Exercises).SingleAsync(p => p.Id == plan.Id);
            Assert.Equal(exercise2.Id, planAfterDelete.Exercises.Single().Id);
        }

        [Fact]
        public async Task Should_Throw_When_Exercise_Does_Not_Belong_To_User()
        {
            // Arrange
            using DbContextFactory factory = new();

            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();
            UserExercise exercise = (await arrangeDbContext.UserExercises.AddAsync(_fixture.Create<UserExercise>())).Entity;
            await arrangeDbContext.SaveChangesAsync();

            // Act / Assert
            using IWorkoutDbContext dbContext = await factory.CreateContext();
            await Assert.ThrowsAsync<KeyNotFoundException>(() => new ExerciseService(dbContext).DeleteExercise(exercise.Id, exercise.UserId + 1));
        }

        [Fact]
        public async Task Should_Throw_When_Exercise_Does_Not_Exist()
        {
            // Arrange
            using DbContextFactory factory = new();

            // Act / Assert
            using IWorkoutDbContext dbContext = await factory.CreateContext();
            await Assert.ThrowsAsync<KeyNotFoundException>(() => new ExerciseService(dbContext).DeleteExercise(1, 1));
        }

        [Fact]
        public async Task Should_Throw_When_Tried_To_Delete_Predefined_Exercise()
        {
            // Arrange
            using DbContextFactory factory = new();

            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();
            PredefinedExercise exercise = (await arrangeDbContext.PredefinedExercises.AddAsync(_fixture.Create<PredefinedExercise>())).Entity;
            await arrangeDbContext.SaveChangesAsync();

            using IWorkoutDbContext dbContext = await factory.CreateContext();

            // Act / Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => new ExerciseService(dbContext).DeleteExercise(exercise.Id, 11));
        }
    }
}
