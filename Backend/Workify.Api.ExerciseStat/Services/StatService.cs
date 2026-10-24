using Microsoft.EntityFrameworkCore;
using Workify.Api.ExerciseStat.Database;
using Workify.Api.ExerciseStat.Models.DTOs;
using Workify.Api.ExerciseStat.Models.DTOs.Parameters;
using Workify.Api.ExerciseStat.Models.Entities;

namespace Workify.Api.ExerciseStat.Services
{
    internal class StatService(IStatDbContext dbContext) : IStatService
    {
        public async Task<IEnumerable<StatDto>> GetAllStats(int userId, CancellationToken ct)
        {
            return await dbContext.Stats.AsNoTracking()
                .Where(s => s.UserId == userId)
                .Select(s => StatDto.FromEntity(s))
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<StatDto>> GetExerciseStats(int userId, int exerciseId, CancellationToken ct)
        {
            return await dbContext.Stats.AsNoTracking()
                .Where(s => s.UserId == userId && s.ExerciseId == exerciseId)
                .Select(s => StatDto.FromEntity(s))
                .ToListAsync(ct);
        }

        public async Task<int> CreateStat(int userId, int exerciseId, CreateEditStatDto dto, CancellationToken ct)
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
            await dbContext.Stats.AddAsync(stat, ct);
            await dbContext.SaveChangesAsync(ct);

            return stat.Id;
        }

        public async Task<int> EditStat(int userId, int statId, CreateEditStatDto dto, CancellationToken ct)
        {
            Stat stat = await dbContext.Stats.FirstOrDefaultAsync(s => s.Id == statId && s.UserId == userId, ct)
                        ?? throw new KeyNotFoundException("No stat with given id and user.");

            stat.Time = dto.Time;
            stat.Reps = dto.Reps;
            stat.Weight = dto.Weight;
            stat.Note = dto.Note;

            await dbContext.SaveChangesAsync(ct);

            return stat.Id;
        }

        public async Task<int> DeleteStat(int userId, int statId, CancellationToken ct)
        {
            Stat stat = await dbContext.Stats.FirstOrDefaultAsync(s => s.Id == statId && s.UserId == userId, ct)
                        ?? throw new KeyNotFoundException("No stat with given id and user.");

            dbContext.Stats.Remove(stat);
            await dbContext.SaveChangesAsync(ct);

            return stat.Id;
        }

        public async Task<IEnumerable<int>> DeleteAllExerciseStats(int userId, int exerciseId, CancellationToken ct)
        {
            List<Stat> stats = await dbContext.Stats
                .Where(s => s.UserId == userId && s.ExerciseId == exerciseId)
                .ToListAsync(ct);

            if (stats.Any())
            {
                dbContext.Stats.RemoveRange(stats);
                await dbContext.SaveChangesAsync(ct);
            }

            return stats.Select(s => s.Id);
        }
    }
}