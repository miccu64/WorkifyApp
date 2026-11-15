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
        private readonly Mock<IPlanService> _planServiceMock = new();
        private readonly Mock<IExerciseService> _exerciseServiceMock = new();

        [Fact]
        public async Task Should_Invoke_Delete_For_All_User_Plans_And_User_Exercises()
        {
            // Arrange
            const int userId = 42;
            List<PlanDto> plans = _fixture.CreateMany<PlanDto>(7).ToList();

            _planServiceMock.Setup(s => s.GetPlans(userId, CancellationToken.None)).ReturnsAsync(plans);

            Mock<ConsumeContext<DeletedUserContract>> mockContext = new();
            mockContext.Setup(c => c.Message).Returns(new DeletedUserContract(userId));

            DeletedUserConsumer consumer = new(_planServiceMock.Object, _exerciseServiceMock.Object);

            // Act
            await consumer.Consume(mockContext.Object);

            // Assert
            _planServiceMock.Verify(service => service.GetPlans(userId, CancellationToken.None), Times.Once);

            foreach (PlanDto plan in plans)
            {
                _planServiceMock.Verify(
                    service => service.DeletePlan(plan.Id, userId, CancellationToken.None),
                    Times.Once
                );
            }

            Assert.Equal(plans.Count + 1, _planServiceMock.Invocations.Count);

            _exerciseServiceMock.Verify(
                service => service.DeleteAllUserExercises(userId, CancellationToken.None),
                Times.Once
            );
        }

        [Fact]
        public async Task Should_Not_Invoke_Delete_When_User_Has_No_Plans()
        {
            // Arrange
            const int userId = 42;

            _planServiceMock.Setup(s => s.GetPlans(userId, CancellationToken.None)).ReturnsAsync([]);

            Mock<ConsumeContext<DeletedUserContract>> mockContext = new();
            mockContext.Setup(c => c.Message).Returns(new DeletedUserContract(userId));

            DeletedUserConsumer consumer = new(_planServiceMock.Object, _exerciseServiceMock.Object);

            // Act
            await consumer.Consume(mockContext.Object);

            // Assert
            _planServiceMock.Verify(service => service.GetPlans(userId, CancellationToken.None), Times.Once);

            _exerciseServiceMock.Verify(
                service => service.DeleteAllUserExercises(userId, CancellationToken.None),
                Times.Once
            );
        }
    }
}