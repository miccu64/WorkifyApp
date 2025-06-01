using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Workify.Api.Workout.Database;

namespace Workify.Api.Workout.UnitTests.Utils
{
    internal class DbContextFactory : IDisposable
    {
        private DbConnection? _connection;

        public async Task<IWorkoutDbContext> CreateContext()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                await _connection.OpenAsync();

                DbContextOptions<WorkoutDbContext> options = CreateOptions();
                await using WorkoutDbContext context = new(options);
                await context.Database.EnsureCreatedAsync();

                context.PredefinedExercises.RemoveRange(context.PredefinedExercises);
                context.PredefinedPlans.RemoveRange(context.PredefinedPlans);
                await context.SaveChangesAsync();
            }

            return new WorkoutDbContext(CreateOptions());
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        private DbContextOptions<WorkoutDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<WorkoutDbContext>()
                .UseSqlite(_connection!).Options;
        }
    }
}
