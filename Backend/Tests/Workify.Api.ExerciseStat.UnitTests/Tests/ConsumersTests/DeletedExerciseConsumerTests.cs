using AutoFixture;
using MassTransit;
using Moq;
using Workify.Api.ExerciseStat.Communication.Consumers;
using Workify.Api.ExerciseStat.Models.DTOs;
using Workify.Api.ExerciseStat.Services;
using Workify.Utils.Communication.Contracts;

namespace Workify.Api.ExerciseStat.UnitTests.Tests.ConsumersTests
{
    public class DeletedExerciseConsumerTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Invoke_DeleteAllExerciseStats_Once()
        {
            // Arrange
            const int userId = 42;
            const int exerciseId = 3;

            Mock<ConsumeContext<DeletedExerciseContract>> mockContext = new();
            mockContext.Setup(c => c.Message).Returns(new DeletedExerciseContract(userId, exerciseId));

            Mock<IStatService> mockStatService = new();
            DeletedExerciseConsumer consumer = new(mockStatService.Object);

            // Act
            await consumer.Consume(mockContext.Object);

            // Assert
            mockStatService.Verify(service => service.DeleteAllExerciseStats(userId, exerciseId), Times.Once);
            Assert.Single(mockStatService.Invocations);
        }
    }
}
