using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Workify.Api.Workout.Database;
using Workify.Api.Workout.Models.DTOs.Parameters;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Services;
using Workify.Api.Workout.UnitTests.Utils;

namespace Workify.Api.Workout.UnitTests.Tests.PlanServiceTests
{
    public class CreatePlanTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Create_Plan()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();
            Exercise exercise = (await arrangeDbContext.Exercises.AddAsync(_fixture.Create<Exercise>())).Entity;
            await arrangeDbContext.SaveChangesAsync();

            CreatePlanDto createPlanDto = _fixture.Build<CreatePlanDto>()
                .With(p => p.ExercisesIds, [exercise.Id])
                .Create();
            const int userId = 11;

            using IWorkoutDbContext dbContext = await factory.CreateContext();

            // Act
            int planId = await new PlanService(dbContext).CreatePlan(userId, createPlanDto);

            // Assert
            Assert.True(planId > 0);

            using IWorkoutDbContext assertDbContext = await factory.CreateContext();
            UserPlan createdPlan = assertDbContext.UserPlans.AsNoTracking()
                .Include(p => p.Exercises)
                .Single(p => p.Id == planId);
            Assert.NotNull(createdPlan);

            Assert.Equal(createPlanDto.Name, createdPlan.Name);
            Assert.Equal(createPlanDto.Description, createdPlan.Description);
            Assert.Equal(userId, createdPlan.UserId);
            Assert.True(createPlanDto.ExercisesIds.Order().SequenceEqual(createdPlan.Exercises.Select(e => e.Id).Order()));
        }
    }
}
