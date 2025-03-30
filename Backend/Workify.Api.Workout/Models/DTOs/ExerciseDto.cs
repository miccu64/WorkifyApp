using Workify.Api.Workout.Models.Others;

namespace Workify.Api.Workout.Models.DTOs
{
    internal record ExerciseDto(int Id, string Name, BodyPartEnum BodyPart, string? Description);
}
