using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Workify.Api.Workout.Models.Entities.Abstractions
{
    internal abstract class Plan
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<Exercise> Exercises { get; } = [];
    }

    internal abstract class BasePlanConfiguration<T> : IConcreteEntityTypeConfiguration<T>
        where T : Plan
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(255);

            ConcreteConfigure(builder);
        }

        public abstract void ConcreteConfigure(EntityTypeBuilder<T> builder);
    }
}
