using Microsoft.EntityFrameworkCore;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Models.Entities.Abstractions;

namespace Workify.Api.Workout.Database
{
    internal class WorkoutDbContext(DbContextOptions<WorkoutDbContext> options) : DbContext(options), IWorkoutDbContext
    {
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PredefinedPlan> PredefinedPlans { get; set; }
        public DbSet<UserPlan> UserPlans { get; set; }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<PredefinedExercise> PredefinedExercises { get; set; }
        public DbSet<UserExercise> UserExercises { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PredefinedPlanConfiguration());
            modelBuilder.ApplyConfiguration(new UserPlanConfiguration());

            modelBuilder.ApplyConfiguration(new PredefinedExerciseConfiguration());
            modelBuilder.ApplyConfiguration(new UserExerciseConfiguration());
        }
    }
}
