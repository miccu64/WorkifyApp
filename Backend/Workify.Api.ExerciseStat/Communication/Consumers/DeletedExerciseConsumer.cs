using MassTransit;

using Workify.Api.ExerciseStat.Services;
using Workify.Utils.Communication.Contracts;

namespace Workify.Api.ExerciseStat.Communication.Consumers
{
    public class DeletedExerciseConsumer(IStatService statService) : IConsumer<DeletedExerciseContract>
    {
        private readonly IStatService _statService = statService;

        public async Task Consume(ConsumeContext<DeletedExerciseContract> context)
        {
            await _statService.DeleteAllExerciseStats(context.Message.UserId, context.Message.ExerciseId);
        }
    }
}
