using Workify.Api.Workout.Database;
using Workify.Api.Workout.Models.DTOs.Parameters;
using Workify.Api.Workout.Models.Entities;

namespace Workify.Api.Workout.Services
{
    internal class PlanService(IWorkoutDbContext workoutDbContext)
    {
        private readonly IWorkoutDbContext _workoutDbContext = workoutDbContext;

        public int AddPlan(int userId, CreatePlanDto dto)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);
            ArgumentNullException.ThrowIfNull(dto);

            return 0;
        }
    }
}
