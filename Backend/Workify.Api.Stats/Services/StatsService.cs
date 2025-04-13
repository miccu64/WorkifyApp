using Microsoft.EntityFrameworkCore;
using Workify.Api.Stats.Database;
using Workify.Api.Stats.Models.DTOs;

namespace Workify.Api.Stats.Services
{
    internal class StatsService(IStatsDbContext dbContext) : IStatsService
    {
        private readonly IStatsDbContext _dbContext = dbContext;

        public async Task<IEnumerable<StatDto>> GetAllStats(int userId)
        {
            return await _dbContext.Stats.AsNoTracking()
                .Where(s => s.UserId == userId)
                .Select(s => StatDto.FromEntity(s))
                .ToListAsync();
        }

        public async Task<IEnumerable<StatDto>> GetExerciseStats(int userId, int exerciseId)
        {
            return await _dbContext.Stats.AsNoTracking()
                .Where(s => s.UserId == userId && s.ExerciseId == exerciseId)
                .Select(s => StatDto.FromEntity(s))
                .ToListAsync();
        }

        public async Task<int> CreateStat(int userId, int exerciseId, CreateStatDto dto)
        {

        }
    }
}
