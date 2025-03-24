using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Workify.Api.Workout.Models.Others;

namespace Workify.Api.Workout.Models.Entities.Abstractions
{
    internal abstract class Exercise
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required BodyPartEnum BodyPart { get; set; }
        public string? Description { get; set; }
        public List<Plan> Plans { get; } = [];
    }

    internal abstract class BaseExerciseConfiguration<T> : IConcreteEntityTypeConfiguration<T>
        where T : Exercise
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
            builder.Property(e => e.BodyPart).IsRequired().HasConversion<int>();

            ConcreteConfigure(builder);
        }

        public abstract void ConcreteConfigure(EntityTypeBuilder<T> builder);
    }
}
