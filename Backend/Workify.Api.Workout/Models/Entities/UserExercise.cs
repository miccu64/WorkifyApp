using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Workify.Api.Workout.Models.Entities
{
    internal class UserExercise : Exercise
    {
        public required int UserId { get; set; }
    }

    internal class UserExerciseConfiguration : IEntityTypeConfiguration<UserExercise>
    {
        public void Configure(EntityTypeBuilder<UserExercise> builder)
        {
            builder.Property(e => e.UserId).IsRequired();
        }
    }
}
