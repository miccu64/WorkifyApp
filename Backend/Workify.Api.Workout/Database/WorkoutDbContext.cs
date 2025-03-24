using Microsoft.EntityFrameworkCore;

namespace Workify.Api.Workout.Database
{
    internal class WorkoutDbContext(DbContextOptions<WorkoutDbContext> options) : DbContext(options), IWorkoutDbContext
    {
        //public DbSet<User> Users { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
