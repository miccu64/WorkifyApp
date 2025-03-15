using Microsoft.EntityFrameworkCore;
using Workify.Api.Auth.Database.Configurations;
using Workify.Api.Auth.Models.Entities;

namespace Workify.Api.Auth.Database
{
    internal class AuthDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
