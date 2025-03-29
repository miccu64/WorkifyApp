using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workify.Api.Workout.Models.Entities.Abstractions;

namespace Workify.Api.Workout.Models.Entities
{
    internal class UserPlan : Plan
    {
        public required int UserId { get; set; }
    }

    internal class UserPlanConfiguration : BasePlanConfiguration<UserPlan>
    {
        public override void ConcreteConfigure(EntityTypeBuilder<UserPlan> builder)
        {
            builder.Property(e => e.UserId).IsRequired();
        }
    }
}
