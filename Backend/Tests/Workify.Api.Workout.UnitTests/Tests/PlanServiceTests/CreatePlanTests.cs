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
        public async Task Should_Create_Plan_With_Exercise_Assignation()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();
            Exercise exercise = (await arrangeDbContext.Exercises.AddAsync(_fixture.Create<Exercise>())).Entity;
            await arrangeDbContext.SaveChangesAsync();

            CreateEditPlanDto createPlanDto = _fixture.Build<CreateEditPlanDto>()
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
                .Single(p => p.Id == planId && p.UserId == userId);
            Assert.NotNull(createdPlan);

            Assert.Equal(createPlanDto.Name, createdPlan.Name);
            Assert.Equal(createPlanDto.Description, createdPlan.Description);
            Assert.Equal(userId, createdPlan.UserId);
            Assert.True(createPlanDto.ExercisesIds.Order().SequenceEqual(createdPlan.Exercises.Select(e => e.Id).Order()));
        }

        [Fact]
        public async Task Should_Create_Plan_Without_Exercise_Assignation()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            CreateEditPlanDto createPlanDto = _fixture.Build<CreateEditPlanDto>()
                .With(p => p.ExercisesIds, [])
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
                .Single(p => p.Id == planId && p.UserId == userId);
            Assert.NotNull(createdPlan);

            Assert.Equal(createPlanDto.Name, createdPlan.Name);
            Assert.Equal(createPlanDto.Description, createdPlan.Description);
            Assert.Equal(userId, createdPlan.UserId);
            Assert.Empty(createPlanDto.ExercisesIds);
        }

        [Fact]
        public async Task Should_Create_Two_Plans_With_Same_Data()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            CreateEditPlanDto createPlanDto = _fixture.Build<CreateEditPlanDto>()
                .With(p => p.ExercisesIds, [])
                .Create();
            const int userId = 11;

            using IWorkoutDbContext dbContext = await factory.CreateContext();

            // Act
            int plan1Id = await new PlanService(dbContext).CreatePlan(userId, createPlanDto);
            int plan2Id = await new PlanService(dbContext).CreatePlan(userId, createPlanDto);

            // Assert
            Assert.True(plan1Id > 0);
            Assert.True(plan2Id > 0);
            Assert.NotEqual(plan1Id, plan2Id);
        }

        [Fact]
        public async Task Should_Throw_When_Exercise_Does_Not_Exist()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            CreateEditPlanDto createPlanDto = _fixture.Create<CreateEditPlanDto>();
            const int userId = 11;

            using IWorkoutDbContext dbContext = await factory.CreateContext();

            // Act / Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => new PlanService(dbContext).CreatePlan(userId, createPlanDto));
        }
    }
}
