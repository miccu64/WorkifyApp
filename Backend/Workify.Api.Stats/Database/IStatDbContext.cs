using Microsoft.EntityFrameworkCore;
using Workify.Api.ExerciseStat.Models.Entities;

namespace Workify.Api.ExerciseStat.Database
{
    internal interface IStatDbContext : IDisposable
    {
        public DbSet<Stat> Stats { get; set; }

        Task<int> SaveChangesAsync();
    }
}
