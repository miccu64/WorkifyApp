using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Workify.Api.ExerciseStat.Database;

namespace Workify.Api.ExerciseStat.UnitTests.Utils
{
    internal class StatDbContextFactory : IDisposable
    {
        private DbConnection? _connection;

        public async Task<IStatDbContext> CreateContext()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                await _connection.OpenAsync();

                DbContextOptions<StatDbContext> options = CreateOptions();
                await using StatDbContext context = new(options);
                await context.Database.EnsureCreatedAsync();
            }

            return new StatDbContext(CreateOptions());
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        private DbContextOptions<StatDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<StatDbContext>()
                .UseSqlite(_connection!).Options;
        }
    }
}
