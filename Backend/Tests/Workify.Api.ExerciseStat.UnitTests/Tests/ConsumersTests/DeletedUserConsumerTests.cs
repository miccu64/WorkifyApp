using AutoFixture;
using MassTransit;
using Moq;
using Workify.Api.ExerciseStat.Communication.Consumers;
using Workify.Api.ExerciseStat.Models.DTOs;
using Workify.Api.ExerciseStat.Services;
using Workify.Utils.Communication.Contracts;

namespace Workify.Api.ExerciseStat.UnitTests.Tests.ConsumersTests
{
    public class DeletedUserConsumerTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Invoke_Delete_For_All_User_Exercises()
        {
            // Arrange
            const int userId = 42;
            List<StatDto> stats1 = _fixture.Build<StatDto>()
                .With(s => s.ExerciseId, 3)
                .CreateMany(3)
                .ToList();
            List<StatDto> stats2 = _fixture.Build<StatDto>()
                .With(s => s.ExerciseId, 12)
                .CreateMany(5)
                .ToList();

            Mock<IStatService> mockStatService = new();
            mockStatService.Setup(s => s.GetAllStats(userId)).ReturnsAsync(stats1.Concat(stats2));

            Mock<ConsumeContext<DeletedUserContract>> mockContext = new();
            mockContext.Setup(c => c.Message).Returns(new DeletedUserContract(userId));

            DeletedUserConsumer consumer = new(mockStatService.Object);

            // Act
            await consumer.Consume(mockContext.Object);

            // Assert
            mockStatService.Verify(service => service.GetAllStats(userId), Times.Once);
            mockStatService.Verify(service => service.DeleteAllExerciseStats(userId, stats1[0].ExerciseId), Times.Once);
            mockStatService.Verify(service => service.DeleteAllExerciseStats(userId, stats2[0].ExerciseId), Times.Once);
            Assert.Equal(3, mockStatService.Invocations.Count);
        }

        [Fact]
        public async Task Should_Not_Invoke_Delete_When_User_Has_Not_Exercises()
        {
            // Arrange
            const int userId = 6;

            Mock<IStatService> mockStatService = new();
            mockStatService.Setup(s => s.GetAllStats(userId)).ReturnsAsync([]);

            Mock<ConsumeContext<DeletedUserContract>> mockContext = new();
            mockContext.Setup(c => c.Message).Returns(new DeletedUserContract(userId));

            DeletedUserConsumer consumer = new(mockStatService.Object);

            // Act
            await consumer.Consume(mockContext.Object);

            // Assert
            mockStatService.Verify(service => service.GetAllStats(userId), Times.Once);
            Assert.Single(mockStatService.Invocations);
        }
    }
}
