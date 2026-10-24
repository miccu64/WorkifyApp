using FluentValidation;
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
        [HttpGet]
        public async Task<IEnumerable<ExerciseDto>> GetExercises()
        {
            int userId = User.GetUserId();

            return await exerciseService.GetExercises(userId);
        }

        [HttpPost]
        public async Task<int> CreateExercise(CreateEditExerciseDto dto,
            CreateEditExerciseDtoValidator validator,
            CancellationToken ct)
        {
            await validator.ValidateAndThrowAsync(dto, ct);

            int userId = User.GetUserId();

            return await exerciseService.CreateExercise(userId, dto);
        }

        [HttpPatch("{exerciseId:int}")]
        public async Task<int> EditExercise(int exerciseId,
            CreateEditExerciseDto dto,
            CreateEditExerciseDtoValidator validator,
            CancellationToken ct)
        {
            await validator.ValidateAndThrowAsync(dto, ct);

            int userId = User.GetUserId();

            return await exerciseService.EditExercise(exerciseId, userId, dto);
        }

        [HttpDelete("{exerciseId:int}")]
        public async Task<int> DeleteExercise(int exerciseId)
        {
            int userId = User.GetUserId();

            return await exerciseService.DeleteExercise(exerciseId, userId);
        }
    }
}