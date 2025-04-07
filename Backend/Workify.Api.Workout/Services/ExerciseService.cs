using Microsoft.EntityFrameworkCore;
using System.Collections;
using Workify.Api.Workout.Database;
using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Models.Entities.Enums;

namespace Workify.Api.Workout.Services
{
    internal class ExerciseService(IWorkoutDbContext workoutDbContext) : IExerciseService
    {
        private readonly IWorkoutDbContext _workoutDbContext = workoutDbContext;

        //public async Task<IEnumerable<ExerciseDto>> GetExercises(int userId)
        //{
        //    return await _workoutDbContext.Exercises.AsNoTracking()
        //        .Where(e => EF.Property<ExerciseTypeEnum>(e, "Type") == ExerciseTypeEnum.Predefined
        //            || (e is UserExercise && ((UserExercise)e).UserId == userId)
        //        ).Select(e => ExerciseDto.FromEntity(e))
        //        .ToListAsync();
        //}

        public async Task<int> CreateExercise()
        {
            return await Task.FromResult(0);
        }

        public async Task<int> EditExercise()
        {
            return await Task.FromResult(0);
        }

        public async Task<int> DeleteExercise()
        {
            return await Task.FromResult(0);
        }
    }
}
