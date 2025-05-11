using MassTransit;
using Workify.Api.Workout.Services;
using Workify.Utils.Communication.Contracts;

namespace Workify.Api.Workout.Communication.Consumers
{
    public class CreatedUserConsumer(IPlanService planService) : IConsumer<CreatedUserContract>
    {
        private readonly IPlanService _planService = planService;

        public async Task Consume(ConsumeContext<CreatedUserContract> context)
        {
            await _planService.CopyPredefinedPlansForUser(context.Message.UserId);
        }
    }
}
