using Workify.Api.Workout.Models.Entities;

namespace Workify.Api.Workout.Models.DTOs
{
    public record PlanDto(int Id, string Name, string? Description, IEnumerable<int> ExercisesIds)
    {
        internal static PlanDto FromEntity(UserPlan p)
            => new(p.Id, p.Name, p.Description, p.Exercises.Select(e => e.Id));
    }
}
