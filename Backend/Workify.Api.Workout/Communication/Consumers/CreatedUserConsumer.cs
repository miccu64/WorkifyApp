using MassTransit;
using Workify.Api.Workout.Services;
using Workify.Utils.Communication.Contracts;

namespace Workify.Api.Workout.Communication.Consumers
{
    public class CreatedUserConsumer(IPlanService planService) : IConsumer<CreatedUserContract>
    {
        public async Task Consume(ConsumeContext<CreatedUserContract> context)
        {
            await planService.CopyPredefinedPlansForUser(context.Message.UserId, context.CancellationToken);
        }
    }
}