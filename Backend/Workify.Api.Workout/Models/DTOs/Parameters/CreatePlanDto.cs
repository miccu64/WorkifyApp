namespace Workify.Api.Workout.Models.DTOs.Parameters
{
    public record CreatePlanDto(string Name, string? Description, IEnumerable<int> ExercisesIds);
}
