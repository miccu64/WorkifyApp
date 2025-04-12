using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Workify.Api.Workout.Database;
using Workify.Api.Workout.Models.DTOs.Parameters;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Services;
using Workify.Api.Workout.UnitTests.Utils;

namespace Workify.Api.Workout.UnitTests.Tests.ServicesTests.PlanServiceTests
{
    public class EditPlanTests
    {
        private readonly Fixture _fixture = EntityFixtureFactory.Create();

        [Fact]
        public async Task Should_Edit_Plan()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();

            PredefinedExercise exercise1 = (await arrangeDbContext.PredefinedExercises.AddAsync(_fixture.Create<PredefinedExercise>())).Entity;
            PredefinedExercise exercise2 = (await arrangeDbContext.PredefinedExercises.AddAsync(_fixture.Create<PredefinedExercise>())).Entity;
            UserPlan plan = _fixture.Build<UserPlan>()
                .With(p => p.Exercises, [exercise1])
                .Create();
            await arrangeDbContext.UserPlans.AddAsync(plan);

            await arrangeDbContext.SaveChangesAsync();

            CreateEditPlanDto editPlanDto = _fixture.Build<CreateEditPlanDto>()
                .With(p => p.ExercisesIds, [exercise2.Id])
                .Create();

            using IWorkoutDbContext dbContext = await factory.CreateContext();

            // Act
            int planId = await new PlanService(dbContext).EditPlan(plan.Id, plan.UserId, editPlanDto);

            // Assert
            Assert.True(planId == plan.Id);

            using IWorkoutDbContext assertDbContext = await factory.CreateContext();
            UserPlan editedPlan = assertDbContext.UserPlans.AsNoTracking()
                .Include(p => p.Exercises)
                .Single(p => p.Id == planId && p.UserId == plan.UserId);
            Assert.NotNull(editedPlan);

            Assert.Equal(editPlanDto.Name, editedPlan.Name);
            Assert.Equal(editPlanDto.Description, editedPlan.Description);
            Assert.Equal(plan.UserId, editedPlan.UserId);
            Assert.True(editPlanDto.ExercisesIds.Order().SequenceEqual(editedPlan.Exercises.Select(e => e.Id).Order()));
        }

        [Fact]
        public async Task Should_Throw_When_Plan_Does_Not_Exist()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();
            using IWorkoutDbContext dbContext = await factory.CreateContext();

            CreateEditPlanDto editPlanDto = _fixture.Create<CreateEditPlanDto>();

            // Act / Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => new PlanService(dbContext).EditPlan(1, 1, editPlanDto));
        }

        [Fact]
        public async Task Should_Throw_When_Plan_Does_Not_Belong_To_User()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();
            UserPlan plan = _fixture.Build<UserPlan>()
                .With(p => p.Exercises, [])
                .Create();
            await arrangeDbContext.UserPlans.AddAsync(plan);
            await arrangeDbContext.SaveChangesAsync();

            using IWorkoutDbContext dbContext = await factory.CreateContext();

            CreateEditPlanDto editPlanDto = _fixture.Create<CreateEditPlanDto>();

            // Act / Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => new PlanService(dbContext).EditPlan(plan.Id, plan.UserId + 1, editPlanDto));
        }
    }
}
