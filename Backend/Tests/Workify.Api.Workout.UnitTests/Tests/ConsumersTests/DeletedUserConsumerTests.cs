using AutoFixture;
using MassTransit;
using Moq;
using Workify.Api.Workout.Communication.Consumers;
using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Services;
using Workify.Utils.Communication.Contracts;

namespace Workify.Api.Workout.UnitTests.Tests.ConsumersTests
{
    public class DeletedUserConsumerTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Invoke_Delete_For_All_User_Plans()
        {
            // Arrange
            const int userId = 42;
            List<PlanDto> plans = _fixture.CreateMany<PlanDto>(7).ToList();

            Mock<IPlanService> mockPlanService = new();
            mockPlanService.Setup(s => s.GetPlans(userId)).ReturnsAsync(plans);

            Mock<ConsumeContext<DeletedUserContract>> mockContext = new();
            mockContext.Setup(c => c.Message).Returns(new DeletedUserContract(userId));

            DeletedUserConsumer consumer = new(mockPlanService.Object);

            // Act
            await consumer.Consume(mockContext.Object);

            // Assert
            mockPlanService.Verify(service => service.GetPlans(userId), Times.Once);

            foreach (PlanDto plan in plans)
                mockPlanService.Verify(service => service.DeletePlan(plan.Id, userId), Times.Once);

            Assert.Equal(plans.Count + 1, mockPlanService.Invocations.Count);
        }

        [Fact]
        public async Task Should_Not_Invoke_Delete_When_User_Has_No_Plans()
        {
            // Arrange
            const int userId = 42;

            Mock<IPlanService> mockPlanService = new();
            mockPlanService.Setup(s => s.GetPlans(userId)).ReturnsAsync([]);

            Mock<ConsumeContext<DeletedUserContract>> mockContext = new();
            mockContext.Setup(c => c.Message).Returns(new DeletedUserContract(userId));

            DeletedUserConsumer consumer = new(mockPlanService.Object);

            // Act
            await consumer.Consume(mockContext.Object);

            // Assert
            mockPlanService.Verify(service => service.GetPlans(userId), Times.Once);
            Assert.Single(mockPlanService.Invocations);
        }
    }
}
