namespace Workify.Api.Stats.Models.Entities
{
    internal class Stat
    {
        public int Id { get; set; }
        public required int ExerciseId { get; set; }
        public required int UserId { get; set; }
        public required DateTime Time { get; set; }
        public required double Weight { get; set; }
        public required int Reps { get; set; }
        public string? Note { get; set; }
    }
}
