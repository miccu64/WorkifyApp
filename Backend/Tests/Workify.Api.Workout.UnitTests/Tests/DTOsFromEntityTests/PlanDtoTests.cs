using AutoFixture;
using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.UnitTests.Utils;

namespace Workify.Api.Workout.UnitTests.Tests.DTOsFromEntityTests
{
    public class PlanDtoTests
    {
        private readonly Fixture _fixture = EntityFixtureFactory.Create();

        [Fact]
        public void Should_Properly_Map_User_Plan()
        {
            // Arrange
            UserPlan plan = _fixture.Create<UserPlan>();

            // Act
            PlanDto dto = PlanDto.FromEntity(plan);

            // Assert
            Assert.Equal(plan.Id, dto.Id);
            Assert.Equal(plan.Name, dto.Name);
            Assert.Equal(plan.Description, dto.Description);
            Assert.True(plan.Exercises.Select(e => e.Id).Order().SequenceEqual(dto.ExercisesIds.Order()));
        }
    }
}
