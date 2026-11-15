using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Workify.Api.Workout.Database;
using Workify.Api.Workout.Models.Entities;
using Workify.Api.Workout.Services;
using Workify.Api.Workout.UnitTests.Utils;

namespace Workify.Api.Workout.UnitTests.Tests.ServicesTests.ExerciseServiceTests;

public class DeleteAllUserExercisesTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task Should_Delete_All_User_Exercises()
    {
        // Arrange
        using DbContextFactory factory = new();

        using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();
        const int userId = 4;
        List<UserExercise> exercises = _fixture.Build<UserExercise>()
            .With(e => e.UserId, userId)
            .CreateMany(3)
            .ToList();
        await arrangeDbContext.UserExercises.AddRangeAsync(exercises);
        await arrangeDbContext.SaveChangesAsync(CancellationToken.None);

        // Act
        using IWorkoutDbContext dbContext = await factory.CreateContext();
        await new ExerciseService(dbContext).DeleteAllUserExercises(userId, CancellationToken.None);

        // Assert
        using IWorkoutDbContext assertDbContext = await factory.CreateContext();
        Assert.Equal(
            0,
            await assertDbContext.UserExercises.CountAsync(e => e.UserId == userId, CancellationToken.None)
        );
    }

    [Fact]
    public async Task Should_Pass_When_No_Exercise_Exists()
    {
        // Arrange
        using DbContextFactory factory = new();

        using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();
        const int userId = 4;

        // Act
        using IWorkoutDbContext dbContext = await factory.CreateContext();
        await new ExerciseService(dbContext).DeleteAllUserExercises(userId, CancellationToken.None);
    }

    [Fact]
    public async Task Should_Delete_Nothing_When_User_Has_No_Own_Exercises()
    {
        // Arrange
        using DbContextFactory factory = new();
        const int otherUserId = 4;
        const int userId = 5;

        using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();

        List<UserExercise> otherUserExercises = _fixture.Build<UserExercise>()
            .With(e => e.UserId, otherUserId)
            .CreateMany(3)
            .ToList();
        await arrangeDbContext.UserExercises.AddRangeAsync(otherUserExercises);

        List<PredefinedExercise> predefinedExercises = _fixture.Build<PredefinedExercise>()
            .CreateMany(3)
            .ToList();
        await arrangeDbContext.PredefinedExercises.AddRangeAsync(predefinedExercises);

        await arrangeDbContext.SaveChangesAsync(CancellationToken.None);

        // Act
        using IWorkoutDbContext dbContext = await factory.CreateContext();
        await new ExerciseService(dbContext).DeleteAllUserExercises(userId, CancellationToken.None);

        // Assert
        using IWorkoutDbContext assertDbContext = await factory.CreateContext();
        Assert.Equal(
            otherUserExercises.Count + predefinedExercises.Count,
            await assertDbContext.AllExercises.CountAsync(CancellationToken.None)
        );
    }

    [Fact]
    public async Task Should_Delete_Only_Given_User_Exercises()
    {
        // Arrange
        using DbContextFactory factory = new();
        const int otherUserId = 4;
        const int userId = 5;

        using IWorkoutDbContext arrangeDbContext = await factory.CreateContext();

        List<UserExercise> otherUserExercises = _fixture.Build<UserExercise>()
            .With(e => e.UserId, otherUserId)
            .CreateMany(3)
            .ToList();
        await arrangeDbContext.UserExercises.AddRangeAsync(otherUserExercises);

        List<UserExercise> userExercises = _fixture.Build<UserExercise>()
            .With(e => e.UserId, userId)
            .CreateMany(3)
            .ToList();
        await arrangeDbContext.UserExercises.AddRangeAsync(userExercises);

        List<PredefinedExercise> predefinedExercises = _fixture.Build<PredefinedExercise>()
            .CreateMany(3)
            .ToList();
        await arrangeDbContext.PredefinedExercises.AddRangeAsync(predefinedExercises);

        await arrangeDbContext.SaveChangesAsync(CancellationToken.None);

        // Act
        using IWorkoutDbContext dbContext = await factory.CreateContext();
        await new ExerciseService(dbContext).DeleteAllUserExercises(userId, CancellationToken.None);

        // Assert
        using IWorkoutDbContext assertDbContext = await factory.CreateContext();

        Assert.Equal(
            otherUserExercises.Count + predefinedExercises.Count,
            await assertDbContext.AllExercises.CountAsync(CancellationToken.None)
        );

        Assert.Equal(
            0,
            await assertDbContext.UserExercises.CountAsync(e => e.UserId == userId, CancellationToken.None)
        );
    }
}