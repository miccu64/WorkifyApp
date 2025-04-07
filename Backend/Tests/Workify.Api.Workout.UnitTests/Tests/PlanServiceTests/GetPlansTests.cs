using AutoFixture;
using Workify.Api.Workout.Database;
using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Models.Entities.Abstractions;
using Workify.Api.Workout.Services;
using Workify.Api.Workout.UnitTests.Utils;

namespace Workify.Api.Workout.UnitTests.Tests.PlanServiceTests
{
    public class GetPlansTests
    {
        private readonly Fixture _fixture = EntityFixtureFactory.Create();

        [Fact]
        public async Task Should_Return_Plan()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();

            UserExercise otherUserExercise = (await arrangeDbContext.UserExercises.AddAsync(_fixture.Create<UserExercise>())).Entity;
            UserPlan otherUserPlan = _fixture.Build<UserPlan>()
                .With(p => p.Exercises, [otherUserExercise])
                .With(p => p.UserId, otherUserExercise.UserId)
                .Create();
            await arrangeDbContext.UserPlans.AddAsync(otherUserPlan);

            const int userId = 11;
            IEnumerable<UserExercise> exercises = _fixture.Build<UserExercise>()
                .With(e => e.UserId, userId)
                .CreateMany(3);
            await arrangeDbContext.UserExercises.AddRangeAsync(exercises);

            UserPlan plan = _fixture.Build<UserPlan>()
                .With(p => p.Exercises, [.. exercises.Cast<Exercise>()])
                .With(p => p.UserId, userId)
                .Create();
            await arrangeDbContext.UserPlans.AddAsync(plan);

            await arrangeDbContext.SaveChangesAsync();

            using IWorkoutDbContext dbContext = await factory.CreateContext();

            // Act
            IEnumerable<PlanDto> plansResponse = await new PlanService(dbContext).GetPlans(userId);

            // Assert
            PlanDto planDto = plansResponse.Single();
            Assert.Equal(planDto.Id, plan.Id);
            Assert.Equal(planDto.Name, plan.Name);
            Assert.Equal(planDto.Description, plan.Description);

            IEnumerable<int> expectedExercisesIds = plan.Exercises.Select(e => e.Id).Order();
            Assert.True(planDto.ExercisesIds.Order().SequenceEqual(expectedExercisesIds));
        }

        [Fact]
        public async Task Should_Return_Empty_Collection_When_Plans_For_User_Id_Do_Not_Exist()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();
            using IWorkoutDbContext dbContext = await factory.CreateContext();
            const int userId = 3;

            // Act
            IEnumerable<PlanDto> plans = await new PlanService(dbContext).GetPlans(userId);

            // Assert
            Assert.Empty(plans);
        }
    }
}
