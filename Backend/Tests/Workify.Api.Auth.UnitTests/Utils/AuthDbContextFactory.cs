using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Workify.Api.Auth.Database;

namespace Workify.Api.Auth.UnitTests.Utils
{
    internal class AuthDbContextFactory : IDisposable
    {
        private DbConnection? _connection;

        public async Task<IAuthDbContext> CreateContext()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");
                await _connection.OpenAsync();

                DbContextOptions<AuthDbContext> options = CreateOptions();
                await using AuthDbContext context = new(options);
                await context.Database.EnsureCreatedAsync();
            }

            return new AuthDbContext(CreateOptions());
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        private DbContextOptions<AuthDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<AuthDbContext>()
                .UseSqlite(_connection!).Options;
        }
    }
}
