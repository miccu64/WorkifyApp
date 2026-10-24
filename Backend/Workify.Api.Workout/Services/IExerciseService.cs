using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Models.DTOs.Parameters;

namespace Workify.Api.Workout.Services
{
    public interface IExerciseService
    {
        Task<IEnumerable<ExerciseDto>> GetExercises(int userId, CancellationToken ct);
        Task<int> CreateExercise(int userId, CreateEditExerciseDto dto, CancellationToken ct);
        Task<int> EditExercise(int exerciseId, int userId, CreateEditExerciseDto dto, CancellationToken ct);
        Task<int> DeleteExercise(int exerciseId, int userId, CancellationToken ct);
    }
}
