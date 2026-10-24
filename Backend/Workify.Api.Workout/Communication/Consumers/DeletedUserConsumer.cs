using MassTransit;
using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Services;
using Workify.Utils.Communication.Contracts;

namespace Workify.Api.Workout.Communication.Consumers
{
    public class DeletedUserConsumer(IPlanService planService) : IConsumer<DeletedUserContract>
    {
        public async Task Consume(ConsumeContext<DeletedUserContract> context)
        {
            int userId = context.Message.UserId;
            IEnumerable<PlanDto> userPlans = await planService.GetPlans(userId, context.CancellationToken);
            foreach (PlanDto planDto in userPlans)
                await planService.DeletePlan(planDto.Id, userId, context.CancellationToken);
        }
    }
}