using Workify.Api.Workout.Models.Others;

namespace Workify.Api.Workout.Models.DTOs.Parameters
{
    internal record CreateEditExerciseDto(string Name, BodyPartEnum BodyPart, string? Description);
}
