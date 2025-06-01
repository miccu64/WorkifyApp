using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Workify.Api.Workout.Database
{
    internal class WorkoutDbContextFactory : IDesignTimeDbContextFactory<WorkoutDbContext>
    {
        public WorkoutDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WorkoutDbContext>();

            optionsBuilder.UseNpgsql("DummyCS");

            return new WorkoutDbContext(optionsBuilder.Options);
        }
    }
}