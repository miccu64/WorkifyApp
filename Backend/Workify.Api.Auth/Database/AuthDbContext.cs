using Microsoft.EntityFrameworkCore;
using Workify.Api.Auth.Models.Entities;

namespace Workify.Api.Auth.Database
{
    internal class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options), IAuthDbContext
    {
        public DbSet<User> Users { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
