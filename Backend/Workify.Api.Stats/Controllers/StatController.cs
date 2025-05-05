using Microsoft.AspNetCore.Mvc;
using Workify.Api.ExerciseStat.Models.DTOs;
using Workify.Api.ExerciseStat.Models.DTOs.Parameters;
using Workify.Api.ExerciseStat.Services;

namespace Workify.Api.ExerciseStat.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/stats")]
    public class StatController(IStatService statService) : ControllerBase
    {
        private readonly IStatService _statService = statService;

        [HttpGet()]
        public async Task<IEnumerable<StatDto>> GetAllStats(int userId)
        {
            return await _statService.GetAllStats(userId);
        }

        [HttpGet("exercises/{exerciseId}")]
        public async Task<IEnumerable<StatDto>> GetExerciseStats(int userId, int exerciseId)
        {
            return await _statService.GetExerciseStats(userId, exerciseId);
        }

        [HttpPost("exercises/{exerciseId}")]
        public async Task<int> CreateStat(int userId, int exerciseId, CreateEditStatDto dto)
        {
            return await _statService.CreateStat(userId, exerciseId, dto);
        }

        [HttpPatch("{statId}")]
        public async Task<int> EditStat(int userId, int statId, CreateEditStatDto dto)
        {
            return await _statService.EditStat(userId, statId, dto);
        }

        [HttpDelete("{statId}")]
        public async Task<int> DeleteStat(int userId, int statId)
        {
            return await _statService.DeleteStat(userId, statId);
        }
    }
}
