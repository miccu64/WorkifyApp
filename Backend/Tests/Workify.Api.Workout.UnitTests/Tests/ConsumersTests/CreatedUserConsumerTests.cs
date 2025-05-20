using MassTransit;
using Moq;
using Workify.Api.Workout.Communication.Consumers;
using Workify.Api.Workout.Services;
using Workify.Utils.Communication.Contracts;

namespace Workify.Api.Workout.UnitTests.Tests.ConsumersTests
{
    public class CreatedUserConsumerTests
    {
        [Fact]
        public async Task Should_Invoke_Copying_Predefined_Plans()
        {
            // Arrange
            const int userId = 22;

            Mock<IPlanService> mockPlanService = new();
            CreatedUserConsumer consumer = new(mockPlanService.Object);

            Mock<ConsumeContext<CreatedUserContract>> mockContext = new();
            mockContext.Setup(c => c.Message).Returns(new CreatedUserContract(userId));

            // Act
            await consumer.Consume(mockContext.Object);

            // Assert
            mockPlanService.Verify(service => service.CopyPredefinedPlansForUser(userId), Times.Once);
            Assert.Single(mockPlanService.Invocations);
        }
    }
}
