using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Models.DTOs.Parameters;
using Workify.Api.Workout.Services;
using Workify.Utils.Communication.Contracts;
using Workify.Utils.Extensions;

namespace Workify.Api.Workout.Controllers
{
    [ApiController]
    [Route("api/exercises")]
    public class ExerciseController(
        IExerciseService exerciseService,
        IPublishEndpoint publishEndpoint
    ) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<ExerciseDto>> GetExercises(CancellationToken ct)
        {
            int userId = User.GetUserId();

            return await exerciseService.GetExercises(userId, ct);
        }

        [HttpPost]
        public async Task<int> CreateExercise(CreateEditExerciseDto dto,
            CreateEditExerciseDtoValidator validator,
            CancellationToken ct)
        {
            await validator.ValidateAndThrowAsync(dto, ct);

            int userId = User.GetUserId();

            return await exerciseService.CreateExercise(userId, dto, ct);
        }

        [HttpPatch("{exerciseId:int}")]
        public async Task<int> EditExercise(int exerciseId,
            CreateEditExerciseDto dto,
            CreateEditExerciseDtoValidator validator,
            CancellationToken ct)
        {
            await validator.ValidateAndThrowAsync(dto, ct);

            int userId = User.GetUserId();

            return await exerciseService.EditExercise(exerciseId, userId, dto, ct);
        }

        [HttpDelete("{exerciseId:int}")]
        public async Task<int> DeleteExercise(int exerciseId, CancellationToken ct)
        {
            int userId = User.GetUserId();

            await exerciseService.DeleteExercise(exerciseId, userId, ct);

            await publishEndpoint.Publish(new DeletedExerciseContract(userId, exerciseId), ct);

            return exerciseId;
        }
    }
}