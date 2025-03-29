using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workify.Api.Workout.Models.Entities.Abstractions;

namespace Workify.Api.Workout.Models.Entities
{
    internal class PredefinedPlan : Plan;

    internal class PredefinedPlanConfiguration : BasePlanConfiguration<PredefinedPlan>
    {
        public override void ConcreteConfigure(EntityTypeBuilder<PredefinedPlan> builder)
        {
        }
    }
}
