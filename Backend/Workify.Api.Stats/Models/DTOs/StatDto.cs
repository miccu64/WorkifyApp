using Workify.Api.ExerciseStat.Models.Entities;

namespace Workify.Api.ExerciseStat.Models.DTOs
{
    public record StatDto(int Id, int ExerciseId, DateTime Time, double Weight, int Reps, string? Note)
    {
        internal static StatDto FromEntity(Stat s) => new(s.Id, s.ExerciseId, s.Time, s.Weight, s.Reps, s.Note);
    }
}
