using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Models.DTOs.Parameters;

namespace Workify.Api.Workout.Services
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanDto>> GetPlans(int userId, CancellationToken ct);
        Task<int> CreatePlan(int userId, CreateEditPlanDto dto, CancellationToken ct);
        Task<int> DeletePlan(int planId, int userId, CancellationToken ct);
        Task<int> EditPlan(int planId, int userId, CreateEditPlanDto dto, CancellationToken ct);
        Task<IEnumerable<int>> CopyPredefinedPlansForUser(int userId, CancellationToken ct);
    }
}
