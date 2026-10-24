using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Models.DTOs.Parameters;
using Workify.Api.Workout.Services;
using Workify.Utils.Extensions;

namespace Workify.Api.Workout.Controllers
{
    [ApiController]
    [Route("api/plans")]
    public class PlanController(IPlanService planService) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<PlanDto>> GetPlans(CancellationToken ct)
        {
            int userId = User.GetUserId();

            return await planService.GetPlans(userId, ct);
        }

        [HttpPost]
        public async Task<int> CreatePlan(CreateEditPlanDto dto,
            CreateEditPlanDtoValidator validator,
            CancellationToken ct)
        {
            await validator.ValidateAndThrowAsync(dto, ct);

            int userId = User.GetUserId();

            return await planService.CreatePlan(userId, dto, ct);
        }

        [HttpPatch("{planId:int}")]
        public async Task<int> EditPlan(int planId,
            CreateEditPlanDto dto,
            CreateEditPlanDtoValidator validator,
            CancellationToken ct)
        {
            await validator.ValidateAndThrowAsync(dto, ct);

            int userId = User.GetUserId();

            return await planService.EditPlan(planId, userId, dto, ct);
        }

        [HttpDelete("{planId:int}")]
        public async Task<int> DeletePlan(int planId, CancellationToken ct)
        {
            int userId = User.GetUserId();

            return await planService.DeletePlan(planId, userId, ct);
        }
    }
}