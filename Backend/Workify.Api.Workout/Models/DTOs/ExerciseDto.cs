using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Models.Others;

namespace Workify.Api.Workout.Models.DTOs
{
    internal record ExerciseDto(int Id, string Name, BodyPartEnum BodyPart, string? Description, bool IsCustom)
    {
        public static ExerciseDto FromEntity(Exercise e) => new(e.Id, e.Name, e.BodyPart, e.Description, e is UserExercise);
    }
}
