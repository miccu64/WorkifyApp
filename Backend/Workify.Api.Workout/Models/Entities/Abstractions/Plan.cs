using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Workify.Api.Workout.Models.Entities.Abstractions
{
    internal abstract class Plan
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<Exercise> Exercises { get; set; } = [];
    }

    internal abstract class BasePlanConfiguration<T> : IConcreteEntityTypeConfiguration<T>
        where T : Plan
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired();

            ConcreteConfigure(builder);
        }

        public abstract void ConcreteConfigure(EntityTypeBuilder<T> builder);
    }
}
