using Workify.Api.ExerciseStat.Models.DTOs;
using Workify.Api.ExerciseStat.Models.DTOs.Parameters;

namespace Workify.Api.ExerciseStat.Services
{
    public interface IStatService
    {
        Task<IEnumerable<StatDto>> GetAllStats(int userId);
        Task<IEnumerable<StatDto>> GetExerciseStats(int userId, int exerciseId);
        Task<int> CreateStat(int userId, int exerciseId, CreateEditStatDto dto);
        Task<int> EditStat(int userId, int statId, CreateEditStatDto dto);
        Task<int> DeleteStat(int userId, int statId);
    }
}
