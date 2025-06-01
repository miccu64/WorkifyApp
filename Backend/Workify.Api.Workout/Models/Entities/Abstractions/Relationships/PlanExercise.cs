namespace Workify.Api.Workout.Models.Entities.Abstractions.Relationships
{
    internal class PlanExercise
    {
        public required int PlanId { get; set; }
        public required int ExerciseId { get; set; }
    }
}