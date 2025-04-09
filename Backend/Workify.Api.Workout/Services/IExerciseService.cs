using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Models.DTOs.Parameters;

namespace Workify.Api.Workout.Services
{
    internal interface IExerciseService
    {
        Task<IEnumerable<ExerciseDto>> GetExercises(int userId);
        Task<int> CreateExercise(int userId, CreateEditExerciseDto dto);
        Task<int> EditExercise(int exerciseId, int userId, CreateEditExerciseDto dto);
        Task<int> DeleteExercise(int exerciseId, int userId);
    }
}
