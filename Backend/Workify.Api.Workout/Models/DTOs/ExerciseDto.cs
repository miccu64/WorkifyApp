using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Models.Entities.Abstractions;
using Workify.Api.Workout.Models.Others;

namespace Workify.Api.Workout.Models.DTOs
{
    public record ExerciseDto(int Id, string Name, BodyPartEnum BodyPart, string? Description, bool IsCustom)
    {
        internal static ExerciseDto FromEntity(Exercise e)
            => new(e.Id, e.Name, e.BodyPart, e.Description, e is UserExercise);
    }
}
