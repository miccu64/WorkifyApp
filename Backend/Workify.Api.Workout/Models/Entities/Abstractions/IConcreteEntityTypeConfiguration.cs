using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Workify.Api.Workout.Models.Entities.Abstractions
{
    internal interface IConcreteEntityTypeConfiguration<T> where T : class
    {
        abstract void ConcreteConfigure(EntityTypeBuilder<T> builder);
    }
}
