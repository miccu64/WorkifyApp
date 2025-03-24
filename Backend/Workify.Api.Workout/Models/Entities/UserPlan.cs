using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Workify.Api.Workout.Models.Entities.Abstractions;

namespace Workify.Api.Workout.Models.Entities
{
    internal class UserPlan : Plan
    {
        public required int UserId { get; set; }
    }

    internal class UserPlanConfiguration : IEntityTypeConfiguration<UserPlan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
            builder.Property(e => e.UserId).IsRequired();
            builder.HasMany(e => e.PredefinedExercises).WithMany(e => e.Plans);
        }
    }
}
