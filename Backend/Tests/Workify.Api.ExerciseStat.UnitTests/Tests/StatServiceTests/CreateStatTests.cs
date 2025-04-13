using AutoFixture;
using Workify.Api.ExerciseStat.Database;
using Workify.Api.ExerciseStat.Models.DTOs.Parameters;
using Workify.Api.ExerciseStat.Models.Entities;
using Workify.Api.ExerciseStat.Services;
using Workify.Api.ExerciseStat.UnitTests.Utils;

namespace Workify.Api.ExerciseStat.UnitTests.Tests.StatServiceTests
{
    public class CreateStatTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Create_Stat()
        {
            // Arrange
            using StatDbContextFactory factory = new();
            using IStatDbContext dbContext = await factory.CreateContext();

            CreateEditStatDto dto = _fixture.Create<CreateEditStatDto>();
            const int userId = 2;
            const int exerciseId = 3;

            // Act
            int createdId = await new StatService(dbContext).CreateStat(userId, exerciseId, dto);

            // Assert
            Assert.True(createdId > 0);

            using IStatDbContext assertDbContext = await factory.CreateContext();
            Stat? stat = await assertDbContext.Stats.FindAsync(createdId);
            Assert.NotNull(stat);

            Assert.Equal(userId, stat.UserId);
            Assert.Equal(exerciseId, stat.ExerciseId);
            Assert.Equal(dto.Time, stat.Time);
            Assert.Equal(dto.Reps, stat.Reps);
            Assert.Equal(dto.Weight, stat.Weight);
            Assert.Equal(dto.Note, stat.Note);
        }

        [Fact]
        public async Task Should_Create_Two_Identical_Stats()
        {
            // Arrange
            using StatDbContextFactory factory = new();
            using IStatDbContext dbContext = await factory.CreateContext();

            CreateEditStatDto dto = _fixture.Create<CreateEditStatDto>();
            const int userId = 2;
            const int exerciseId = 3;

            // Act
            int createdId1 = await new StatService(dbContext).CreateStat(userId, exerciseId, dto);
            int createdId2 = await new StatService(dbContext).CreateStat(userId, exerciseId, dto);

            // Assert
            Assert.True(createdId1 > 0);
            Assert.True(createdId2 > 0);
            Assert.NotEqual(createdId1, createdId2);
        }
    }
}
