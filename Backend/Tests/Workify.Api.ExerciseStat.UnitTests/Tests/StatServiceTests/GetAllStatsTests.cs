using AutoFixture;
using Workify.Api.ExerciseStat.Database;
using Workify.Api.ExerciseStat.Models.DTOs;
using Workify.Api.ExerciseStat.Models.Entities;
using Workify.Api.ExerciseStat.Services;
using Workify.Api.ExerciseStat.UnitTests.Utils;

namespace Workify.Api.ExerciseStat.UnitTests.Tests.StatServiceTests
{
    public class GetAllStatsTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Return_Empty_List_When_User_Has_No_Stats()
        {
            // Arrange
            using StatDbContextFactory factory = new();

            using IStatDbContext arrangeDbContext = await factory.CreateContext();
            Stat stat = _fixture.Create<Stat>();
            await arrangeDbContext.Stats.AddAsync(stat);
            await arrangeDbContext.SaveChangesAsync();

            using IStatDbContext dbContext = await factory.CreateContext();

            // Act
            IEnumerable<StatDto> stats = await new StatService(dbContext).GetAllStats(stat.UserId + 1);

            // Assert
            Assert.Empty(stats);
        }

        [Fact]
        public async Task Should_Return_User_Stats()
        {
            // Arrange
            using StatDbContextFactory factory = new();

            using IStatDbContext arrangeDbContext = await factory.CreateContext();
            const int userId = 3;
            List<Stat> stats = _fixture.Build<Stat>()
                .With(s => s.UserId, userId)
                .CreateMany(5)
                .ToList();
            await arrangeDbContext.Stats.AddRangeAsync(stats);
            await arrangeDbContext.SaveChangesAsync();

            using IStatDbContext dbContext = await factory.CreateContext();

            // Act
            IEnumerable<StatDto> responseStats = await new StatService(dbContext).GetAllStats(userId);

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
