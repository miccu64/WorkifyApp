using Workify.Api.Stats.Models.Entities;

namespace Workify.Api.Stats.Models.DTOs
{
    internal record StatDto(int Id, int ExerciseId, DateTime Time, double Weight, int Reps, string? Note)
    {
        public static StatDto FromEntity(Stat s) => new(s.Id, s.ExerciseId, s.Time, s.Weight, s.Reps, s.Note);
    }
}
