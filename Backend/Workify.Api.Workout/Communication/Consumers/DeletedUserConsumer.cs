using MassTransit;
using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Services;
using Workify.Utils.Communication.Contracts;

namespace Workify.Api.Workout.Communication.Consumers
{
    public class DeletedUserConsumer(IPlanService planService) : IConsumer<DeletedUserContract>
    {
        private readonly IPlanService _planService = planService;

        public async Task Consume(ConsumeContext<DeletedUserContract> context)
        {
            int userId = context.Message.UserId;
            IEnumerable<PlanDto> userPlans = await _planService.GetPlans(userId);
            foreach (PlanDto planDto in userPlans)
                await _planService.DeletePlan(planDto.Id, userId);
        }
    }
}
