using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workify.Api.Workout.Models.Entities.Abstractions;

namespace Workify.Api.Workout.Models.Entities
{
    internal class UserExercise : Exercise
    {
        public required int UserId { get; set; }
    }

    internal class UserExerciseConfiguration : BaseExerciseConfiguration<UserExercise>
    {
        public override void ConcreteConfigure(EntityTypeBuilder<UserExercise> builder)
        {
            builder.Property(e => e.UserId).IsRequired();
        }
    }
}
