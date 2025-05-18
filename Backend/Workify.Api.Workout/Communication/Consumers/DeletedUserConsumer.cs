using MassTransit;
using Workify.Api.Workout.Services;
using Workify.Utils.Communication.Contracts;

namespace Workify.Api.Workout.Communication.Consumers
{
    public class DeletedUserConsumer(IPlanService planService) : IConsumer<DeletedUserContract>
    {
        private readonly IPlanService _planService = planService;

        public async Task Consume(ConsumeContext<DeletedUserContract> context)
        {
            // TODO: delete user plans
        }
    }
}
