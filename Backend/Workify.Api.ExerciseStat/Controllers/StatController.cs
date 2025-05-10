using Microsoft.AspNetCore.Mvc;

using Workify.Api.ExerciseStat.Models.DTOs;
using Workify.Api.ExerciseStat.Models.DTOs.Parameters;
using Workify.Api.ExerciseStat.Services;
using Workify.Utils.Extensions;

namespace Workify.Api.ExerciseStat.Controllers
{
    [ApiController]
    [Route("api/stats")]
    public class StatController(IStatService statService) : ControllerBase
    {
        private readonly IStatService _statService = statService;

        [HttpGet]
        public async Task<IEnumerable<StatDto>> GetAllStats()
        {
            int userId = User.GetUserId();

            return await _statService.GetAllStats(userId);
        }

        [HttpGet("exercises/{exerciseId}")]
        public async Task<IEnumerable<StatDto>> GetExerciseStats(int exerciseId)
        {
            int userId = User.GetUserId();

            return await _statService.GetExerciseStats(userId, exerciseId);
        }

        [HttpPost("exercises/{exerciseId}")]
        public async Task<int> CreateStat(int exerciseId, CreateEditStatDto dto)
        {
            int userId = User.GetUserId();

            return await _statService.CreateStat(userId, exerciseId, dto);
        }

        [HttpPatch("{statId}")]
        public async Task<int> EditStat(int statId, CreateEditStatDto dto)
        {
            int userId = User.GetUserId();

            return await _statService.EditStat(userId, statId, dto);
        }

        [HttpDelete("{statId}")]
        public async Task<int> DeleteStat(int statId)
        {
            int userId = User.GetUserId();

            return await _statService.DeleteStat(userId, statId);
        }
    }
}
