namespace Workify.Api.Workout.Models.DTOs
{
    // TODO: consider making static mappers in DTOs
    internal record PlanDto(int Id, string Name, string? Description, IEnumerable<int> ExercisesIds);
}
