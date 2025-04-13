using Microsoft.EntityFrameworkCore;
using Workify.Api.Stats.Models.Entities;

namespace Workify.Api.Stats.Database
{
    internal class StatsDbContext(DbContextOptions<StatsDbContext> options) : DbContext(options), IStatsDbContext
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
