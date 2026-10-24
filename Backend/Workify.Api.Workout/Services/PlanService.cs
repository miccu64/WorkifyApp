using Microsoft.EntityFrameworkCore;
using Workify.Api.Workout.Database;
using Workify.Api.Workout.Models.DTOs;
using Workify.Api.Workout.Models.DTOs.Parameters;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Models.Entities.Abstractions;

namespace Workify.Api.Workout.Services
{
    internal class PlanService(IWorkoutDbContext workoutDbContext) : IPlanService
    {
        public async Task<IEnumerable<PlanDto>> GetPlans(int userId, CancellationToken ct)
        {
            return await workoutDbContext.UserPlans.AsNoTracking()
                .Include(p => p.Exercises)
                .Where(p => p.UserId == userId)
                .Select(p => PlanDto.FromEntity(p))
                .ToListAsync(ct);
        }

        public async Task<int> CreatePlan(int userId, CreateEditPlanDto dto, CancellationToken ct)
        {
            UserPlan plan = new()
            {
                Name = dto.Name,
                UserId = userId,
                Description = dto.Description,
                Exercises = await GetExercisesToAdd(userId, dto, ct)
            };

            await workoutDbContext.UserPlans.AddAsync(plan, ct);
            await workoutDbContext.SaveChangesAsync(ct);

            return plan.Id;
        }

        public async Task<int> DeletePlan(int planId, int userId, CancellationToken ct)
        {
            UserPlan plan = await workoutDbContext.UserPlans.AsNoTracking()
                                .SingleOrDefaultAsync(p => p.Id == planId && p.UserId == userId, ct)
                            ?? throw new ArgumentException("Plan with given id with given user id does not exist.");

            workoutDbContext.UserPlans.Remove(plan);
            await workoutDbContext.SaveChangesAsync(ct);

            return plan.Id;
        }

        public async Task<int> EditPlan(int planId, int userId, CreateEditPlanDto dto, CancellationToken ct)
        {
            UserPlan plan = await workoutDbContext.UserPlans
                                .Include(p => p.Exercises)
                                .SingleOrDefaultAsync(p => p.Id == planId && p.UserId == userId, ct)
                            ?? throw new KeyNotFoundException("Plan with given id with given user id does not exist.");

            plan.Name = dto.Name;
            plan.Description = dto.Description;
            plan.Exercises = await GetExercisesToAdd(userId, dto, ct);

            await workoutDbContext.SaveChangesAsync(ct);

            return plan.Id;
        }

        public async Task<IEnumerable<int>> CopyPredefinedPlansForUser(int userId, CancellationToken ct)
        {
            List<UserPlan> predefinedUserPlans = (await workoutDbContext.PredefinedPlans
                    .Include(p => p.Exercises)
                    .ToListAsync(ct)
                ).ConvertAll(p => new UserPlan
                {
                    UserId = userId,
                    Name = p.Name,
                    Description = p.Description,
                    Exercises = p.Exercises
                });

            await workoutDbContext.UserPlans.AddRangeAsync(predefinedUserPlans, ct);
            await workoutDbContext.SaveChangesAsync(ct);

            return predefinedUserPlans.Select(p => p.Id);
        }

        private async Task<List<Exercise>> GetExercisesToAdd(int userId, CreateEditPlanDto dto, CancellationToken ct)
        {
            List<Exercise> exercisesToAdd = [];
            int exercisesCount = dto.ExercisesIds.Count();
            if (exercisesCount > 0)
            {
                exercisesToAdd.AddRange(await workoutDbContext.PredefinedExercises
                    .Where(e => dto.ExercisesIds.Contains(e.Id))
                    .ToListAsync(ct)
                );
                exercisesToAdd.AddRange(await workoutDbContext.UserExercises
                    .Where(e => e.UserId == userId && dto.ExercisesIds.Contains(e.Id))
                    .ToListAsync(ct));

                if (exercisesToAdd.Count != exercisesCount)
                    throw new KeyNotFoundException("Improper exercises ids.");
            }

            return exercisesToAdd;
        }
    }
}