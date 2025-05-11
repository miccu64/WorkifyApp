using Microsoft.EntityFrameworkCore;

using Workify.Api.ExerciseStat.Database;
using Workify.Api.ExerciseStat.Models.DTOs;
using Workify.Api.ExerciseStat.Models.DTOs.Parameters;
using Workify.Api.ExerciseStat.Models.Entities;

namespace Workify.Api.ExerciseStat.Services
{
    internal class StatService(IStatDbContext dbContext) : IStatService
    {
        private readonly IStatDbContext _dbContext = dbContext;

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

        public async Task<int> CreateStat(int userId, int exerciseId, CreateEditStatDto dto)
        {
            Stat stat = new()
            {
                ExerciseId = exerciseId,
                UserId = userId,
                Time = dto.Time,
                Reps = dto.Reps,
                Weight = dto.Weight,
                Note = dto.Note
            };
            await _dbContext.Stats.AddAsync(stat);
            await _dbContext.SaveChangesAsync();

            return stat.Id;
        }

        public async Task<int> EditStat(int userId, int statId, CreateEditStatDto dto)
        {
            Stat stat = await _dbContext.Stats.FirstOrDefaultAsync(s => s.Id == statId && s.UserId == userId)
                ?? throw new KeyNotFoundException("No stat with given id and user.");

            stat.Time = dto.Time;
            stat.Reps = dto.Reps;
            stat.Weight = dto.Weight;
            stat.Note = dto.Note;

            await _dbContext.SaveChangesAsync();

            return stat.Id;
        }

        public async Task<int> DeleteStat(int userId, int statId)
        {
            Stat stat = await _dbContext.Stats.FirstOrDefaultAsync(s => s.Id == statId && s.UserId == userId)
                ?? throw new KeyNotFoundException("No stat with given id and user.");

            _dbContext.Stats.Remove(stat);
            await _dbContext.SaveChangesAsync();

            return stat.Id;
        }

        public async Task<IEnumerable<int>> DeleteAllExerciseStats(int userId, int exerciseId)
        {
            List<Stat> stats = await _dbContext.Stats.Where(s => s.UserId == userId && s.ExerciseId == exerciseId)
                .ToListAsync();

            if (stats.Any())
            {
                _dbContext.Stats.RemoveRange(stats);
                await _dbContext.SaveChangesAsync();
            }

            return stats.Select(s => s.Id);
        }
    }
}
