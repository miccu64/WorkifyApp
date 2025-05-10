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
        private readonly IPlanService _planService = planService;

        [HttpGet]
        public async Task<IEnumerable<PlanDto>> GetPlans()
        {
            int userId = User.GetUserId();

            return await _planService.GetPlans(userId);
        }

        [HttpPost]
        public async Task<int> CreatePlan(CreateEditPlanDto dto)
        {
            int userId = User.GetUserId();

            return await _planService.CreatePlan(userId, dto);
        }

        [HttpPatch("{planId}")]
        public async Task<int> EditPlan(int planId, CreateEditPlanDto dto)
        {
            int userId = User.GetUserId();

            return await _planService.EditPlan(planId, userId, dto);
        }

        [HttpDelete("{planId}")]
        public async Task<int> DeletePlan(int planId)
        {
            int userId = User.GetUserId();

            return await _planService.DeletePlan(planId, userId);
        }
    }
}
