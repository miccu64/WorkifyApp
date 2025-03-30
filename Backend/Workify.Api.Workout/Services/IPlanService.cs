using Workify.Api.Workout.Models.DTOs.Parameters;

namespace Workify.Api.Workout.Services
{
    internal interface IPlanService
    {
        Task<int> AddPlan(CreatePlanDto dto, int userId);
        Task<int> DeletePlan(int planId, int userId);
        Task<int> EditPlan(int planId, EditPlanDto dto, int userId);
    }
}
