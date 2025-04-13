using AutoFixture;
using Workify.Api.ExerciseStat.Database;
using Workify.Api.ExerciseStat.Models.Entities;
using Workify.Api.ExerciseStat.Services;
using Workify.Api.ExerciseStat.UnitTests.Utils;

namespace Workify.Api.ExerciseStat.UnitTests.Tests.StatServiceTests
{
    public class DeleteStatTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Delete_Stat()
        {
            // Arrange
            using StatDbContextFactory factory = new();

            using IStatDbContext arrangeDbContext = await factory.CreateContext();
            Stat stat = _fixture.Create<Stat>();
            await arrangeDbContext.Stats.AddAsync(stat);
            await arrangeDbContext.SaveChangesAsync();

            using IStatDbContext dbContext = await factory.CreateContext();

            // Act
            int deletedId = await new StatService(dbContext).DeleteStat(stat.UserId, stat.Id);

            // Assert
            Assert.Equal(stat.Id, deletedId);

            using IStatDbContext assertDbContext = await factory.CreateContext();
            Stat? deletedStat = await assertDbContext.Stats.FindAsync(deletedId);
            Assert.Null(deletedStat);
        }

        [Fact]
        public async Task Should_Throw_When_Stat_Does_Not_Exist()
        {
            // Arrange
            using StatDbContextFactory factory = new();
            using IStatDbContext dbContext = await factory.CreateContext();

            // Act / Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => new StatService(dbContext).DeleteStat(1, 1));
        }

        [Fact]
        public async Task Should_Throw_When_User_Is_Not_Owner_Of_Stat()
        {
            // Arrange
            using StatDbContextFactory factory = new();

            using IStatDbContext arrangeDbContext = await factory.CreateContext();
            Stat stat = (await arrangeDbContext.Stats.AddAsync(_fixture.Create<Stat>())).Entity;
            await arrangeDbContext.SaveChangesAsync();

            using IStatDbContext dbContext = await factory.CreateContext();

            // Act / Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => new StatService(dbContext).DeleteStat(stat.UserId + 1, stat.Id));
        }
    }
}
