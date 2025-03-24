using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workify.Api.Workout.Models.Entities.Abstractions;

namespace Workify.Api.Workout.Models.Entities
{
    internal class PredefinedExercise : Exercise
    {
    }

    internal class PredefinedExerciseConfiguration : BaseExerciseConfiguration<PredefinedExercise>
    {
        public override void ConcreteConfigure(EntityTypeBuilder<PredefinedExercise> builder)
        {
        }
    }
}
