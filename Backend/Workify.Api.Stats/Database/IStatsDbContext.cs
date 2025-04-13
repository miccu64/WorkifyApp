using Microsoft.EntityFrameworkCore;
using Workify.Api.Stats.Models.Entities;

namespace Workify.Api.Stats.Database
{
    internal interface IStatsDbContext
    {
        public DbSet<Stat> Stats { get; set; }

        Task<int> SaveChangesAsync();
    }
}
