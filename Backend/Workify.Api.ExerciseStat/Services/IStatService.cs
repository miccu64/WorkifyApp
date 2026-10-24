using Workify.Api.ExerciseStat.Models.DTOs;
using Workify.Api.ExerciseStat.Models.DTOs.Parameters;

namespace Workify.Api.ExerciseStat.Services
{
    public interface IStatService
    {
        Task<IEnumerable<StatDto>> GetAllStats(int userId, CancellationToken ct);
        Task<IEnumerable<StatDto>> GetExerciseStats(int userId, int exerciseId, CancellationToken ct);
        Task<int> CreateStat(int userId, int exerciseId, CreateEditStatDto dto, CancellationToken ct);
        Task<int> EditStat(int userId, int statId, CreateEditStatDto dto, CancellationToken ct);
        Task<int> DeleteStat(int userId, int statId, CancellationToken ct);
        Task<IEnumerable<int>> DeleteAllExerciseStats(int userId, int exerciseId, CancellationToken ct);
    }
}
