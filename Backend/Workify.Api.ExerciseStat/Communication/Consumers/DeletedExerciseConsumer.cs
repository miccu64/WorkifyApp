using MassTransit;
using Workify.Api.ExerciseStat.Services;
using Workify.Utils.Communication.Contracts;

namespace Workify.Api.ExerciseStat.Communication.Consumers
{
    public class DeletedExerciseConsumer(IStatService statService) : IConsumer<DeletedExerciseContract>
    {
        public async Task Consume(ConsumeContext<DeletedExerciseContract> context)
        {
            await statService.DeleteAllExerciseStats(context.Message.UserId, context.Message.ExerciseId, context.CancellationToken);
        }
    }
}
