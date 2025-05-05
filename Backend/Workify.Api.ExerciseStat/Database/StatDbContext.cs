using Microsoft.EntityFrameworkCore;
using Workify.Api.ExerciseStat.Models.Entities;

namespace Workify.Api.ExerciseStat.Database
{
    internal class StatDbContext(DbContextOptions<StatDbContext> options) : DbContext(options), IStatDbContext
    {
        public DbSet<Stat> Stats { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
