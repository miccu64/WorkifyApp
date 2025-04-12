using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Workify.Api.Workout.Database;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Models.Entities.Abstractions;
using Workify.Api.Workout.Services;
using Workify.Api.Workout.UnitTests.Utils;

namespace Workify.Api.Workout.UnitTests.Tests.ServicesTests.PlanServiceTests
{
    public class DeletePlanTests
    {
        private readonly Fixture _fixture = EntityFixtureFactory.Create();

        [Fact]
        public async Task Should_Delete_Plan_And_Leave_Exercises()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();

            PredefinedExercise exercise = (await arrangeDbContext.PredefinedExercises.AddAsync(_fixture.Create<PredefinedExercise>())).Entity;
            UserPlan plan = _fixture.Create<UserPlan>();
            plan.Exercises = [exercise];
            await arrangeDbContext.UserPlans.AddAsync(plan);

            await arrangeDbContext.SaveChangesAsync();

            using IWorkoutDbContext dbContext = await factory.CreateContext();

            // Act
            int deletedPlanId = await new PlanService(dbContext).DeletePlan(plan.Id, plan.UserId);

            // Assert
            Assert.True(deletedPlanId == plan.Id);

            using IWorkoutDbContext assertDbContext = await factory.CreateContext();
            UserPlan? notExistingPlan = await assertDbContext.UserPlans.SingleOrDefaultAsync(p => p.Id == deletedPlanId);
            Assert.Null(notExistingPlan);

            Exercise existingExercise = await assertDbContext.AllExercises.SingleAsync(e => e.Id == exercise.Id);
            Assert.NotNull(existingExercise);
        }

        [Fact]
        public async Task Should_Throw_When_User_Is_Not_Owner_Of_Plan()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();
            UserPlan plan = (await arrangeDbContext.UserPlans.AddAsync(_fixture.Create<UserPlan>())).Entity;
            await arrangeDbContext.SaveChangesAsync();

            using IWorkoutDbContext dbContext = await factory.CreateContext();

            // Act / Assert
            int otherUserId = plan.UserId + 1;
            await Assert.ThrowsAsync<ArgumentException>(() => new PlanService(dbContext).DeletePlan(plan.Id, otherUserId));
        }

        [Fact]
        public async Task Should_Throw_When_Plan_Does_Not_Exist()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();
            UserPlan plan = (await arrangeDbContext.UserPlans.AddAsync(_fixture.Create<UserPlan>())).Entity;
            await arrangeDbContext.SaveChangesAsync();

            using IWorkoutDbContext dbContext = await factory.CreateContext();

            // Act / Assert
            int notExistingPlanId = plan.Id + 1;
            await Assert.ThrowsAsync<ArgumentException>(() => new PlanService(dbContext).DeletePlan(notExistingPlanId, plan.UserId));
        }
    }
}
