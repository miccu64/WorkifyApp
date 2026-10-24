using FluentValidation;
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
        [HttpGet]
        public async Task<IEnumerable<StatDto>> GetAllStats()
        {
            int userId = User.GetUserId();

            return await statService.GetAllStats(userId);
        }

        [HttpGet("exercises/{exerciseId:int}")]
        public async Task<IEnumerable<StatDto>> GetExerciseStats(int exerciseId)
        {
            int userId = User.GetUserId();

            return await statService.GetExerciseStats(userId, exerciseId);
        }

        [HttpPost("exercises/{exerciseId:int}")]
        public async Task<int> CreateStat(int exerciseId,
            CreateEditStatDto dto,
            CreateEditStatDtoValidator validator,
            CancellationToken ct)
        {
            await validator.ValidateAndThrowAsync(dto, ct);

            int userId = User.GetUserId();

            return await statService.CreateStat(userId, exerciseId, dto);
        }

        [HttpPatch("{statId:int}")]
        public async Task<int> EditStat(int statId,
            CreateEditStatDto dto,
            CreateEditStatDtoValidator validator,
            CancellationToken ct)
        {
            await validator.ValidateAndThrowAsync(dto, ct);

            int userId = User.GetUserId();

            return await statService.EditStat(userId, statId, dto);
        }

        [HttpDelete("{statId:int}")]
        public async Task<int> DeleteStat(int statId)
        {
            int userId = User.GetUserId();

            return await statService.DeleteStat(userId, statId);
        }
    }
}