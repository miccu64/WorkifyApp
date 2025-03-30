namespace Workify.Api.Workout.Models.DTOs.Parameters
{
    public record EditPlanDto(string Name, string? Description, IEnumerable<int> ExercisesIds);
}
