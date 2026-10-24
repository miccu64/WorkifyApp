using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Workify.Api.Workout.Database
{
    internal class WorkoutDbContextFactory : IDesignTimeDbContextFactory<WorkoutDbContext>
    {
        public WorkoutDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<WorkoutDbContext> optionsBuilder = new();

            optionsBuilder.UseNpgsql("DummyCS");

            return new WorkoutDbContext(optionsBuilder.Options);
        }
    }
}