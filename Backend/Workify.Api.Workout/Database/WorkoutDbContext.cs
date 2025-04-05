using Microsoft.EntityFrameworkCore;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Models.Entities.Abstractions;
using Workify.Api.Workout.Models.Entities.Enums;

namespace Workify.Api.Workout.Database
{
    internal class WorkoutDbContext(DbContextOptions<WorkoutDbContext> options) : DbContext(options), IWorkoutDbContext
    {
        public DbSet<PredefinedPlan> PredefinedPlans { get; set; }
        public DbSet<UserPlan> UserPlans { get; set; }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<UserExercise> UserExercises { get; set; }


        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plan>().UseTpcMappingStrategy();
            modelBuilder.Entity<PredefinedPlan>().ToTable("PredefinedPlans");
            modelBuilder.Entity<UserPlan>().ToTable("UserPlans");

            modelBuilder.Entity<Exercise>().HasDiscriminator<ExerciseTypeEnum>("ExerciseType")
                .HasValue<Exercise>(ExerciseTypeEnum.Predefined)
                .HasValue<UserExercise>(ExerciseTypeEnum.User);

            modelBuilder.ApplyConfiguration(new PlanConfiguration());
            modelBuilder.ApplyConfiguration(new UserPlanConfiguration());

            modelBuilder.ApplyConfiguration(new ExerciseConfiguration());
            modelBuilder.ApplyConfiguration(new UserExerciseConfiguration());
        }
    }
}
