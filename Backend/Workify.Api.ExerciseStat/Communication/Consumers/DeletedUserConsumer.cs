using MassTransit;
using Workify.Api.ExerciseStat.Models.DTOs;
using Workify.Api.ExerciseStat.Services;
using Workify.Utils.Communication.Contracts;

namespace Workify.Api.ExerciseStat.Communication.Consumers
{
    public class DeletedUserConsumer(IStatService statService) : IConsumer<DeletedUserContract>
    {
        public async Task Consume(ConsumeContext<DeletedUserContract> context)
        {
            int userId = context.Message.UserId;
            IEnumerable<StatDto> userStats = await statService.GetAllStats(userId, context.CancellationToken);
            IEnumerable<int> exercisesIds = userStats.Select(stat => stat.ExerciseId).Distinct();
            foreach (int exerciseId in exercisesIds)
                await statService.DeleteAllExerciseStats(userId, exerciseId, context.CancellationToken);
        }
    }
}