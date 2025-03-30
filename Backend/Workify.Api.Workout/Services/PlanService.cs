using Microsoft.EntityFrameworkCore;
using Workify.Api.Workout.Database;
using Workify.Api.Workout.Models.DTOs.Parameters;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Models.Entities.Abstractions;

namespace Workify.Api.Workout.Services
{
    internal class PlanService(IWorkoutDbContext workoutDbContext) : IPlanService
    {
        private readonly IWorkoutDbContext _workoutDbContext = workoutDbContext;

        public async Task<int> AddPlan(CreatePlanDto dto, int userId)
        {
            List<Exercise> exercisesToAdd = [];
            int exercisesCount = dto.ExercisesIds.Count();
            if (exercisesCount > 0)
            {
                exercisesToAdd = await _workoutDbContext.Exercises
                    .Where(e => dto.ExercisesIds.Contains(e.Id))
                    .ToListAsync();
                if (exercisesToAdd.Count != exercisesCount)
                    throw new KeyNotFoundException("Not all exercises exist.");
            }

            UserPlan plan = new()
            {
                Name = dto.Name,
                UserId = userId,
                Description = dto.Description,
                Exercises = exercisesToAdd
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

        public async Task<int> EditPlan(int planId, EditPlanDto dto, int userId)
        {
            UserPlan plan = await _workoutDbContext.UserPlans.SingleOrDefaultAsync(p => p.Id == planId && p.UserId == userId)
                ?? throw new KeyNotFoundException("Plan with given id with given user id does not exist.");

            int exercisesCount = dto.ExercisesIds.Count();
            List<Exercise> exercisesToAdd = [];
            if (exercisesCount > 0)
            {
                exercisesToAdd = await _workoutDbContext.Exercises
                    .Where(e => dto.ExercisesIds.Contains(e.Id))
                    .ToListAsync();
                if (exercisesToAdd.Count != exercisesCount)
                    throw new KeyNotFoundException("Not all exercises exist.");
            }

            plan.Name = dto.Name;
            plan.Description = dto.Description;
            plan.Exercises = exercisesToAdd;

            await _workoutDbContext.SaveChangesAsync();

            return plan.Id;
        }
    }
}
