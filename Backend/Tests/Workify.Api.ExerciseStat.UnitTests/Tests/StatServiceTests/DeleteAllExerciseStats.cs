using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Workify.Api.ExerciseStat.Database;
using Workify.Api.ExerciseStat.Models.Entities;
using Workify.Api.ExerciseStat.Services;
using Workify.Api.ExerciseStat.UnitTests.Utils;

namespace Workify.Api.ExerciseStat.UnitTests.Tests.StatServiceTests
{
    public class DeleteAllExerciseStats
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Delete_All_Exercise_Stats_For_User()
        {
            // Arrange
            using StatDbContextFactory factory = new();

            using IStatDbContext arrangeDbContext = await factory.CreateContext();

            const int exerciseId = 3;
            const int userId = 3;
            List<Stat> stats = _fixture.Build<Stat>()
                .With(s => s.ExerciseId, exerciseId)
                .With(s => s.UserId, userId)
                .CreateMany()
                .ToList();
            await arrangeDbContext.Stats.AddRangeAsync(stats);

            await arrangeDbContext.SaveChangesAsync();

            using IStatDbContext dbContext = await factory.CreateContext();

            // Act
            IEnumerable<int> deletedIds = await new StatService(dbContext).DeleteAllExerciseStats(userId, exerciseId);

            // Assert
            Assert.True(stats.Select(s => s.Id).Order().SequenceEqual(deletedIds.Order()));

            using IStatDbContext assertDbContext = await factory.CreateContext();
            int dbStatsCount = await assertDbContext.Stats.CountAsync();
            Assert.Equal(0, dbStatsCount);
        }

        [Fact]
        public async Task Should_Delete_Exercise_Stats_Only_For_Given_User()
        {
            // Arrange
            using StatDbContextFactory factory = new();

            using IStatDbContext arrangeDbContext = await factory.CreateContext();

            const int exerciseId = 3;

            const int userIdToDelete = 1;
            Stat statToDelete = _fixture.Build<Stat>()
                .With(s => s.ExerciseId, exerciseId)
                .With(s => s.UserId, userIdToDelete)
                .Create();
            await arrangeDbContext.Stats.AddAsync(statToDelete);

            const int userId2 = 22;
            Stat otherUserStat = _fixture.Build<Stat>()
                 .With(s => s.ExerciseId, exerciseId)
                 .With(s => s.UserId, userId2)
                 .Create();
            await arrangeDbContext.Stats.AddAsync(otherUserStat);

            await arrangeDbContext.SaveChangesAsync();

            using IStatDbContext dbContext = await factory.CreateContext();

            // Act
            IEnumerable<int> deletedIds = await new StatService(dbContext).DeleteAllExerciseStats(userIdToDelete, exerciseId);

            // Assert
            Assert.Equal(statToDelete.Id, deletedIds.Single());

            using IStatDbContext assertDbContext = await factory.CreateContext();

            bool isStatDeleted = (await assertDbContext.Stats.FindAsync(statToDelete.Id)) == null;
            Assert.True(isStatDeleted);

            bool otherUserStatExists = (await assertDbContext.Stats.FindAsync(otherUserStat.Id)) != null;
            Assert.True(otherUserStatExists);
        }

        [Fact]
        public async Task Should_Delete_Exercise_Stats_Only_For_Given_Exercise()
        {
            // Arrange
            using StatDbContextFactory factory = new();

            using IStatDbContext arrangeDbContext = await factory.CreateContext();

            const int userId = 1;
            List<Stat> stats = _fixture.Build<Stat>()
                 .With(s => s.UserId, userId)
                 .CreateMany(11)
                 .ToList();
            await arrangeDbContext.Stats.AddRangeAsync(stats);

            await arrangeDbContext.SaveChangesAsync();

            Stat statToDelete = stats[0];

            using IStatDbContext dbContext = await factory.CreateContext();

            // Act
            IEnumerable<int> deletedIds = await new StatService(dbContext).DeleteAllExerciseStats(userId, statToDelete.ExerciseId);

            // Assert
            Assert.Equal(statToDelete.Id, deletedIds.Single());

            using IStatDbContext assertDbContext = await factory.CreateContext();

            bool isStatDeleted = (await assertDbContext.Stats.FindAsync(statToDelete.Id)) == null;
            Assert.True(isStatDeleted);

            bool otherStatsExists = await assertDbContext.Stats.CountAsync() == (stats.Count - 1);
            Assert.True(otherStatsExists);
        }

        [Fact]
        public async Task Should_Delete_Nothing_When_User_Has_Not_Stats_For_Exercise()
        {
            // Arrange
            using StatDbContextFactory factory = new();
            using IStatDbContext dbContext = await factory.CreateContext();

            // Act
            IEnumerable<int> deletedIds = await new StatService(dbContext).DeleteAllExerciseStats(3, 3);

            // Assert
            Assert.Empty(deletedIds);
        }
    }
}
