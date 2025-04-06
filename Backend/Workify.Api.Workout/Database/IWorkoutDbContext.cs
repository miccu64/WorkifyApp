using Microsoft.EntityFrameworkCore;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Models.Entities.Abstractions;

namespace Workify.Api.Workout.Database
{
    internal interface IWorkoutDbContext : IDisposable
    {
        DbSet<PredefinedPlan> PredefinedPlans { get; set; }
        DbSet<UserPlan> UserPlans { get; set; }

        DbSet<Exercise> Exercises { get; set; }
        DbSet<UserExercise> UserExercises { get; set; }

        Task<int> SaveChangesAsync();
    }
}
