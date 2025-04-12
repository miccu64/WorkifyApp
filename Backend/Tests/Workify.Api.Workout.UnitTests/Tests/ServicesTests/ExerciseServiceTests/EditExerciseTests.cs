using AutoFixture;
using Workify.Api.Workout.Database;
using Workify.Api.Workout.Models.DTOs.Parameters;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Services;
using Workify.Api.Workout.UnitTests.Utils;

namespace Workify.Api.Workout.UnitTests.Tests.ServicesTests.ExerciseServiceTests
{
    public class EditExerciseTests
    {
        private readonly Fixture _fixture = FixtureFactory.ExerciseRelayCreate();

        [Fact]
        public async Task Should_Edit_Exercise()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();
            UserExercise exercise = (await arrangeDbContext.UserExercises.AddAsync(_fixture.Create<UserExercise>())).Entity;
            await arrangeDbContext.SaveChangesAsync();

            CreateEditExerciseDto dto = _fixture.Create<CreateEditExerciseDto>();

            // Act
            using IWorkoutDbContext dbContext = await factory.CreateContext();
            int editedExerciseId = await new ExerciseService(dbContext).EditExercise(exercise.Id, exercise.UserId, dto);

            // Assert
            Assert.Equal(exercise.Id, editedExerciseId);

            using IWorkoutDbContext assertDbContext = await factory.CreateContext();
            UserExercise? exerciseAfterEdit = await assertDbContext.UserExercises.FindAsync(exercise.Id);

            Assert.NotNull(exerciseAfterEdit);
            Assert.Equal(dto.Name, exerciseAfterEdit.Name);
            Assert.Equal(dto.Description, exerciseAfterEdit.Description);
            Assert.Equal(dto.BodyPart, exerciseAfterEdit.BodyPart);
        }

        [Fact]
        public async Task Should_Throw_When_Exercise_Is_Predefined()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();
            PredefinedExercise exercise = (await arrangeDbContext.PredefinedExercises.AddAsync(_fixture.Create<PredefinedExercise>())).Entity;
            await arrangeDbContext.SaveChangesAsync();

            CreateEditExerciseDto dto = _fixture.Create<CreateEditExerciseDto>();

            // Act / Assert
            using IWorkoutDbContext dbContext = await factory.CreateContext();
            await Assert.ThrowsAsync<KeyNotFoundException>(()=>new ExerciseService(dbContext).EditExercise(exercise.Id, 1, dto));
        }

        [Fact]
        public async Task Should_Throw_When_User_Is_Not_Exercise_Owner()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();
            UserExercise exercise = (await arrangeDbContext.UserExercises.AddAsync(_fixture.Create<UserExercise>())).Entity;
            await arrangeDbContext.SaveChangesAsync();

            CreateEditExerciseDto dto = _fixture.Create<CreateEditExerciseDto>();

            // Act / Assert
            using IWorkoutDbContext dbContext = await factory.CreateContext();
            await Assert.ThrowsAsync<KeyNotFoundException>(() => new ExerciseService(dbContext).EditExercise(exercise.Id, exercise.UserId + 1, dto));
        }

        [Fact]
        public async Task Should_Throw_When_Exercise_Does_Not_Exist()
        {
            // Arrange
            using WorkoutDbContextFactory factory = new();

            CreateEditExerciseDto dto = _fixture.Create<CreateEditExerciseDto>();

            // Act / Assert
            using IWorkoutDbContext dbContext = await factory.CreateContext();
            await Assert.ThrowsAsync<KeyNotFoundException>(() => new ExerciseService(dbContext).EditExercise(1, 1, dto));
        }
    }
}
