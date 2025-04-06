using Microsoft.EntityFrameworkCore;
using Workify.Api.Workout.Database;
using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Models.DTOs.Parameters;
using Workify.Api.Workout.Models.Entities;

namespace Workify.Api.Workout.Services
{
    internal class PlanService(IWorkoutDbContext workoutDbContext) : IPlanService
    {
        private readonly IWorkoutDbContext _workoutDbContext = workoutDbContext;

        public async Task<IEnumerable<PlanDto>> GetPlans(int userId)
        {
            return await _workoutDbContext.UserPlans.AsNoTracking()
                .Where(p => p.UserId == userId)
                .Select(p => new PlanDto(p.Id, p.Name, p.Description, p.Exercises.Select(e => e.Id)))
                .ToListAsync();
        }

        public async Task<int> CreatePlan(int userId, CreateEditPlanDto dto)
        {
            UserPlan plan = new()
            {
                Name = dto.Name,
                UserId = userId,
                Description = dto.Description,
                Exercises = await GetExercisesToAdd(userId, dto)
            };

            await _workoutDbContext.UserPlans.AddAsync(plan);
            await _workoutDbContext.SaveChangesAsync();

            return plan.Id;
        }

        public async Task<int> DeletePlan(int planId, int userId)
        {
            UserPlan plan = await _workoutDbContext.UserPlans.AsNoTracking().SingleOrDefaultAsync(p => p.Id == planId && p.UserId == userId)
                ?? throw new ArgumentException("Plan with given id with given user id does not exist.");

            _workoutDbContext.UserPlans.Remove(plan);
            await _workoutDbContext.SaveChangesAsync();

            return plan.Id;
        }

        public async Task<int> EditPlan(int planId, int userId, CreateEditPlanDto dto)
        {
            UserPlan plan = await _workoutDbContext.UserPlans
                .Include(p => p.Exercises)
                .SingleOrDefaultAsync(p => p.Id == planId && p.UserId == userId)
                ?? throw new KeyNotFoundException("Plan with given id with given user id does not exist.");

            plan.Name = dto.Name;
            plan.Description = dto.Description;
            plan.Exercises = await GetExercisesToAdd(userId, dto);

            await _workoutDbContext.SaveChangesAsync();

            return plan.Id;
        }

        private async Task<List<Exercise>> GetExercisesToAdd(int userId, CreateEditPlanDto dto)
        {
            List<Exercise> exercisesToAdd = [];
            int exercisesCount = dto.ExercisesIds.Count();
            if (exercisesCount > 0)
            {
                exercisesToAdd = [.. (await _workoutDbContext.Exercises
                        .Where(e => dto.ExercisesIds.Contains(e.Id))
                        .ToListAsync()
                    ).Where(e => e is not UserExercise userExercise || userExercise.UserId == userId)];

                if (exercisesToAdd.Count != exercisesCount)
                    throw new KeyNotFoundException("Improper exercises ids.");
            }

            return exercisesToAdd;
        }
    }
}
