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
        private readonly IWorkoutDbContext _workoutDbContext = workoutDbContext;

        public async Task<IEnumerable<ExerciseDto>> GetExercises(int userId)
        {
            return (await _workoutDbContext.PredefinedExercises.AsNoTracking()
                .Cast<Exercise>()
                .ToListAsync()
            ).Union(await _workoutDbContext.UserExercises.AsNoTracking()
                .Where(e => e.UserId == userId)
                .ToListAsync()
            ).Select(ExerciseDto.FromEntity);
        }

        public async Task<int> CreateExercise(int userId, CreateEditExerciseDto dto)
        {
            UserExercise exercise = new()
            {
                Name = dto.Name,
                Description = dto.Description,
                BodyPart = dto.BodyPart,
                UserId = userId
            };
            await _workoutDbContext.UserExercises.AddAsync(exercise);
            await _workoutDbContext.SaveChangesAsync();

            return exercise.Id;
        }

        public async Task<int> EditExercise(int exerciseId, int userId, CreateEditExerciseDto dto)
        {
            UserExercise exercise = await _workoutDbContext.UserExercises.SingleOrDefaultAsync(e => e.Id == exerciseId && e.UserId == userId)
                ?? throw new KeyNotFoundException("No exercise with given user id and exercise id.");

            exercise.Name = dto.Name;
            exercise.Description = dto.Description;
            exercise.BodyPart = dto.BodyPart;

            await _workoutDbContext.SaveChangesAsync();

            return exercise.Id;
        }

        public async Task<int> DeleteExercise(int exerciseId, int userId)
        {
            UserExercise exercise = await _workoutDbContext.UserExercises.SingleOrDefaultAsync(e => e.Id == exerciseId && e.UserId == userId)
                 ?? throw new KeyNotFoundException("No exercise with given user id and exercise id.");

            _workoutDbContext.UserExercises.Remove(exercise);
            await _workoutDbContext.SaveChangesAsync();

            return exercise.Id;
        }
    }
}
