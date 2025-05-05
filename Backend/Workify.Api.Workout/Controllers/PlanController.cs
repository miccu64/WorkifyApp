using Microsoft.AspNetCore.Mvc;
using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Models.DTOs.Parameters;
using Workify.Api.Workout.Services;

namespace Workify.Api.Workout.Controllers
{
    // TODO: replace userId in requests with userId from JWT token
    [ApiController]
    [Route("api/users/{userId}/plans")]
    public class PlanController(IPlanService planService) : ControllerBase
    {
        private readonly IPlanService _planService = planService;

        [HttpGet]
        public async Task<IEnumerable<PlanDto>> GetPlans(int userId)
        {
            return await _planService.GetPlans(userId);
        }

        [HttpPost]
        public async Task<int> CreatePlan(int userId, CreateEditPlanDto dto)
        {
            return await _planService.CreatePlan(userId, dto);
        }

        [HttpPatch("{planId}")]
        public async Task<int> EditPlan(int planId, int userId, CreateEditPlanDto dto)
        {
            return await _planService.EditPlan(planId, userId, dto);
        }

        [HttpDelete("{planId}")]
        public async Task<int> DeletePlan(int planId, int userId)
        {
            return await _planService.DeletePlan(planId, userId);
        }
    }
}
