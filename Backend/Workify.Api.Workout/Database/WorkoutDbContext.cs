using Microsoft.EntityFrameworkCore;

using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Models.Entities.Abstractions;
using Workify.Api.Workout.Models.Entities.Abstractions.Relationships;
using Workify.Api.Workout.Models.Entities.Enums;
using Workify.Api.Workout.Models.Others;

namespace Workify.Api.Workout.Database
{
    internal class WorkoutDbContext(DbContextOptions<WorkoutDbContext> options) : DbContext(options), IWorkoutDbContext
    {
        public DbSet<PredefinedPlan> PredefinedPlans { get; set; }
        public DbSet<UserPlan> UserPlans { get; set; }

        public DbSet<Exercise> AllExercises { get; set; }
        public DbSet<PredefinedExercise> PredefinedExercises { get; set; }
        public DbSet<UserExercise> UserExercises { get; set; }


        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plan>().UseTptMappingStrategy();
            modelBuilder.Entity<PredefinedPlan>().ToTable("PredefinedPlans");
            modelBuilder.Entity<UserPlan>().ToTable("UserPlans");

            modelBuilder.Entity<Exercise>().HasDiscriminator<ExerciseTypeEnum>("ExerciseType")
                .HasValue<PredefinedExercise>(ExerciseTypeEnum.Predefined)
                .HasValue<UserExercise>(ExerciseTypeEnum.User);

            modelBuilder.ApplyConfiguration(new PlanConfiguration());
            modelBuilder.ApplyConfiguration(new UserPlanConfiguration());

            modelBuilder.ApplyConfiguration(new ExerciseConfiguration());
            modelBuilder.ApplyConfiguration(new UserExerciseConfiguration());

            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            List<PredefinedExercise> exercises = [
                new() { Id = 1, BodyPart = BodyPartEnum.Chest, Name = "Bench press" },
                new() { Id = 2, BodyPart = BodyPartEnum.Legs, Name = "Squat" },
                new() { Id = 3, BodyPart = BodyPartEnum.Legs, Name = "Deadlift" },
                new() { Id = 4, BodyPart = BodyPartEnum.Back, Name = "Rows" }
            ];
            List<PredefinedPlan> plans = [
                new() { Id = 1, Name = "FBW" },
                new() { Id = 2, Name = "Upper body" },
                new() { Id = 3, Name = "Lower body" }
            ];

            modelBuilder.Entity<PredefinedExercise>().HasData(exercises);
            modelBuilder.Entity<PredefinedPlan>().HasData(plans);

            Dictionary<int, IEnumerable<int>> planExerciseRelationships = new()
            {
                { plans[0].Id, exercises.Select(e => e.Id) },
                { plans[1].Id, [exercises[0].Id, exercises[3].Id] },
                { plans[2].Id, [exercises[1].Id, exercises[2].Id] },
            };
            IEnumerable<PlanExercise> planExerciseList = planExerciseRelationships.SelectMany(x =>
                x.Value.Select(exerciseId => new PlanExercise() { PlanId = x.Key, ExerciseId = exerciseId })
            );
            modelBuilder.Entity<PlanExercise>().HasData(planExerciseList);
        }
    }
}
