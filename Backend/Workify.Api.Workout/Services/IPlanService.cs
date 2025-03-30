using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Models.DTOs.Parameters;

namespace Workify.Api.Workout.Services
{
    internal interface IPlanService
    {
        Task<IEnumerable<PlanDto>> GetPlans(int userId);
        Task<int> CreatePlan(CreatePlanDto dto, int userId);
        Task<int> DeletePlan(int planId, int userId);
        Task<int> EditPlan(int planId, EditPlanDto dto, int userId);
    }
}
