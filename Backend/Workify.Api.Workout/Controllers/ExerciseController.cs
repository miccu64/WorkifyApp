using Microsoft.AspNetCore.Mvc;
using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Models.DTOs.Parameters;
using Workify.Api.Workout.Services;
using Workify.Utils.Extensions;

namespace Workify.Api.Workout.Controllers
{
    [ApiController]
    [Route("api/exercises")]
    public class ExerciseController(IExerciseService exerciseService) : ControllerBase
    {
        private readonly IExerciseService _exerciseService = exerciseService;

        [HttpGet]
        public async Task<IEnumerable<ExerciseDto>> GetExercises()
        {
            int userId = User.GetUserId();

            return await _exerciseService.GetExercises(userId);
        }

        [HttpPost]
        public async Task<int> CreateExercise(CreateEditExerciseDto dto)
        {
            int userId = User.GetUserId();

            return await _exerciseService.CreateExercise(userId, dto);
        }

        [HttpPatch("{exerciseId}")]
        public async Task<int> EditExercise(int exerciseId, CreateEditExerciseDto dto)
        {
            int userId = User.GetUserId();

            return await _exerciseService.EditExercise(exerciseId, userId, dto);
        }

        [HttpDelete("{exerciseId}")]
        public async Task<int> DeleteExercise(int exerciseId)
        {
            int userId = User.GetUserId();

            return await _exerciseService.DeleteExercise(exerciseId, userId);
        }
    }
}
