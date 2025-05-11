using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Workify.Api.Workout.Database;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Models.Entities.Abstractions;
using Workify.Api.Workout.Services;
using Workify.Api.Workout.UnitTests.Utils;

namespace Workify.Api.Workout.UnitTests.Tests.ServicesTests.PlanServiceTests
{
    public class CopyPredefinedPlansForUserTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Copy_Predefined_Plans_For_User()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();

            List<PredefinedExercise> exercises = _fixture.Build<PredefinedExercise>()
                .CreateMany(3)
                .ToList();
            await arrangeDbContext.PredefinedExercises.AddRangeAsync(exercises);

            List<PredefinedPlan> plans = _fixture.Build<PredefinedPlan>()
                .With(p => p.Exercises, [.. exercises.Cast<Exercise>()])
                .CreateMany(3)
                .ToList();
            await arrangeDbContext.PredefinedPlans.AddRangeAsync(plans);

            await arrangeDbContext.SaveChangesAsync();

            const int userId = 22;

            using IWorkoutDbContext dbContext = await factory.CreateContext();

            // Act
            IEnumerable<int> response = await new PlanService(dbContext).CopyPredefinedPlansForUser(userId);

            // Assert
            Assert.Equal(plans.Count, response.Count());
            Assert.DoesNotContain(plans, p => response.Contains(p.Id));
        }

        [Fact]
        public async Task Should_Copy_Predefined_Plan_And_Assign_Its_Exercises_For_User()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();

            List<PredefinedExercise> exercises = _fixture.Build<PredefinedExercise>()
                .CreateMany(3)
                .ToList();
            await arrangeDbContext.PredefinedExercises.AddRangeAsync(exercises);

            PredefinedPlan plan = _fixture.Build<PredefinedPlan>()
                .With(p => p.Exercises, [.. exercises.Cast<Exercise>()])
                .Create();
            await arrangeDbContext.PredefinedPlans.AddAsync(plan);

            await arrangeDbContext.SaveChangesAsync();

            const int userId = 22;

            using IWorkoutDbContext dbContext = await factory.CreateContext();

            // Act
            IEnumerable<int> response = await new PlanService(dbContext).CopyPredefinedPlansForUser(userId);

            // Assert
            Assert.NotEqual(plan.Id, response.Single());

            using IWorkoutDbContext assertDbContext = await factory.CreateContext();

            UserPlan createdPlan = await assertDbContext.UserPlans.AsNoTracking()
                .Include(p => p.Exercises)
                .SingleAsync();
            Assert.Equal(response.Single(), createdPlan.Id);
            Assert.Equal(plan.Name, createdPlan.Name);
            Assert.Equal(plan.Description, createdPlan.Description);
            Assert.Equal(userId, createdPlan.UserId);
            Assert.True(exercises.Select(e => e.Id).Order().SequenceEqual(createdPlan.Exercises.Select(e => e.Id).Order()));
        }

        [Fact]
        public async Task Should_Return_Empty_Result_When_No_Predefined_Plans_Exist()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            const int userId = 22;

            using IWorkoutDbContext dbContext = await factory.CreateContext();

            // Act
            IEnumerable<int> response = await new PlanService(dbContext).CopyPredefinedPlansForUser(userId);

            // Assert
            Assert.Empty(response);
        }
    }
}
