using Microsoft.EntityFrameworkCore;
using Workify.Api.Auth.Models.Entities;

namespace Workify.Api.Auth.Database
{
    internal class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options), IAuthDbContext
    {
        public DbSet<User> Users { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return await base.SaveChangesAsync(ct);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}