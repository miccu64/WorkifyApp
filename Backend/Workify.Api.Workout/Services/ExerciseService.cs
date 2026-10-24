using Microsoft.EntityFrameworkCore;
using Workify.Api.Workout.Database;
using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Models.DTOs.Parameters;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Models.Entities.Abstractions;

namespace Workify.Api.Workout.Services
{
    internal class ExerciseService(IWorkoutDbContext workoutDbContext) : IExerciseService
    {
        public async Task<IEnumerable<ExerciseDto>> GetExercises(int userId, CancellationToken ct)
        {
            return (await workoutDbContext.PredefinedExercises.AsNoTracking()
                    .Cast<Exercise>()
                    .ToListAsync(ct)
                ).Union(await workoutDbContext.UserExercises.AsNoTracking()
                    .Where(e => e.UserId == userId)
                    .ToListAsync(ct)
                ).Select(ExerciseDto.FromEntity);
        }

        public async Task<int> CreateExercise(int userId, CreateEditExerciseDto dto, CancellationToken ct)
        {
            UserExercise exercise = new()
            {
                Name = dto.Name,
                Description = dto.Description,
                BodyPart = dto.BodyPart,
                UserId = userId
            };
            await workoutDbContext.UserExercises.AddAsync(exercise, ct);
            await workoutDbContext.SaveChangesAsync(ct);

            return exercise.Id;
        }

        public async Task<int> EditExercise(int exerciseId, int userId, CreateEditExerciseDto dto, CancellationToken ct)
        {
            UserExercise exercise =
                await workoutDbContext.UserExercises.SingleOrDefaultAsync(
                    e => e.Id == exerciseId && e.UserId == userId,
                    ct
                ) ?? throw new KeyNotFoundException("No exercise with given user id and exercise id.");

            exercise.Name = dto.Name;
            exercise.Description = dto.Description;
            exercise.BodyPart = dto.BodyPart;

            await workoutDbContext.SaveChangesAsync(ct);

            return exercise.Id;
        }

        public async Task<int> DeleteExercise(int exerciseId, int userId, CancellationToken ct)
        {
            UserExercise exercise =
                await workoutDbContext.UserExercises.SingleOrDefaultAsync(
                    e => e.Id == exerciseId && e.UserId == userId,
                    ct
                ) ?? throw new KeyNotFoundException("No exercise with given user id and exercise id.");

            workoutDbContext.UserExercises.Remove(exercise);
            await workoutDbContext.SaveChangesAsync(ct);

            return exercise.Id;
        }
    }
}