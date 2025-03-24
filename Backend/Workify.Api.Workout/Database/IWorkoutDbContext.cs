using Microsoft.EntityFrameworkCore;

namespace Workify.Api.Workout.Database
{
    internal interface IWorkoutDbContext : IDisposable
    {
        //public DbSet<User> Users { get; set; }

        public Task<int> SaveChangesAsync();
    }
}
