using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workify.Api.Workout.Models.Others;

namespace Workify.Api.Workout.Models.Entities.Abstractions
{
    internal abstract class Exercise
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required BodyPartEnum BodyPart { get; set; }
        public string? Description { get; set; }
    }

    internal class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.BodyPart).IsRequired().HasConversion<int>();
        }
    }
}
