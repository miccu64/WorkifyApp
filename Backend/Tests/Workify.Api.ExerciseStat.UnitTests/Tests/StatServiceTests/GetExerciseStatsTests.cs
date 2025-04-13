using AutoFixture;
using Workify.Api.ExerciseStat.Database;
using Workify.Api.ExerciseStat.Models.DTOs;
using Workify.Api.ExerciseStat.Models.Entities;
using Workify.Api.ExerciseStat.Services;
using Workify.Api.ExerciseStat.UnitTests.Utils;

namespace Workify.Api.ExerciseStat.UnitTests.Tests.StatServiceTests
{
    public class GetExerciseStatsTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Return_Empty_List_When_User_Has_No_Stats_For_Exercise()
        {
            // Arrange
            using StatDbContextFactory factory = new();

            using IStatDbContext arrangeDbContext = await factory.CreateContext();
            const int userId = 3;
            const int exerciseId = 4;

            Stat userStatForDifferentExercise = _fixture.Build<Stat>()
                .With(s => s.UserId, userId)
                .With(s => s.ExerciseId, exerciseId + 1)
                .Create();
            await arrangeDbContext.Stats.AddAsync(userStatForDifferentExercise);

            Stat otherUserStat = _fixture.Build<Stat>()
                .With(s => s.UserId, userId + 1)
                .With(s => s.ExerciseId, exerciseId)
                .Create();
            await arrangeDbContext.Stats.AddAsync(otherUserStat);

            await arrangeDbContext.SaveChangesAsync();

            using IStatDbContext dbContext = await factory.CreateContext();

            // Act
            IEnumerable<StatDto> stats = await new StatService(dbContext).GetExerciseStats(userId, exerciseId);

            // Assert
            Assert.Empty(stats);
        }

        [Fact]
        public async Task Should_Return_User_Stats_For_Exercise()
        {
            // Arrange
            using StatDbContextFactory factory = new();

            using IStatDbContext arrangeDbContext = await factory.CreateContext();
            const int userId = 3;
            const int exerciseId = 4;

            List<Stat> stats = _fixture.Build<Stat>()
                .With(s => s.UserId, userId)
                .With(s => s.ExerciseId, exerciseId)
                .CreateMany(5)
                .ToList();
            await arrangeDbContext.Stats.AddRangeAsync(stats);

            List<Stat> statsForDifferentExercise = _fixture.Build<Stat>()
                .With(s => s.UserId, userId)
                .With(s => s.ExerciseId, exerciseId + 3)
                .CreateMany(3)
                .ToList();
            await arrangeDbContext.Stats.AddRangeAsync(statsForDifferentExercise);

            await arrangeDbContext.SaveChangesAsync();

            using IStatDbContext dbContext = await factory.CreateContext();

            // Act
            IEnumerable<StatDto> responseStats = await new StatService(dbContext).GetExerciseStats(userId, exerciseId);

            // Assert
            Assert.Equal(stats.Count, responseStats.Count());
            foreach (StatDto dto in responseStats)
            {
                Stat stat = stats.Single(s => s.Id == dto.Id);
                Assert.Equal(stat.ExerciseId, dto.ExerciseId);
                Assert.Equal(stat.Weight, dto.Weight);
                Assert.Equal(stat.Time, dto.Time);
                Assert.Equal(stat.Reps, dto.Reps);
                Assert.Equal(stat.Note, dto.Note);
            }
        }
    }
}
