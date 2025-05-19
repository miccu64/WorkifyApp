using MassTransit;
using Workify.Api.ExerciseStat.Models.DTOs;
using Workify.Api.ExerciseStat.Services;
using Workify.Utils.Communication.Contracts;

namespace Workify.Api.ExerciseStat.Communication.Consumers
{
    public class DeletedUserConsumer(IStatService statService) : IConsumer<DeletedUserContract>
    {
        private readonly IStatService _statService = statService;

        public async Task Consume(ConsumeContext<DeletedUserContract> context)
        {
            int userId = context.Message.UserId;
            IEnumerable<StatDto> userStats = await _statService.GetAllStats(userId);
            IEnumerable<int> exercisesIds = userStats.Select(stat => stat.ExerciseId).Distinct();
            foreach (int exerciseId in exercisesIds)
                await _statService.DeleteAllExerciseStats(userId, exerciseId);
        }
    }
}
