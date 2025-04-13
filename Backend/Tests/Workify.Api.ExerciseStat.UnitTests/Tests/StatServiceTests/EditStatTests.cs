using AutoFixture;
using Workify.Api.ExerciseStat.Database;
using Workify.Api.ExerciseStat.Models.DTOs.Parameters;
using Workify.Api.ExerciseStat.Models.Entities;
using Workify.Api.ExerciseStat.Services;
using Workify.Api.ExerciseStat.UnitTests.Utils;

namespace Workify.Api.ExerciseStat.UnitTests.Tests.StatServiceTests
{
    public class EditStatTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Edit_Stat()
        {
            // Arrange
            using StatDbContextFactory factory = new();

            using IStatDbContext arrangeDbContext = await factory.CreateContext();
            Stat stat = (await arrangeDbContext.Stats.AddAsync(_fixture.Create<Stat>())).Entity;
            await arrangeDbContext.SaveChangesAsync();

            CreateEditStatDto dto = _fixture.Create<CreateEditStatDto>();

            using IStatDbContext dbContext = await factory.CreateContext();

            // Act
            int editedId = await new StatService(dbContext).EditStat(stat.UserId, stat.Id, dto);

            // Assert
            Assert.Equal(stat.Id, editedId);

            using IStatDbContext assertDbContext = await factory.CreateContext();
            Stat? editedStat = await assertDbContext.Stats.FindAsync(editedId);
            Assert.NotNull(editedStat);

            Assert.Equal(stat.UserId, editedStat.UserId);
            Assert.Equal(stat.ExerciseId, editedStat.ExerciseId);
            Assert.Equal(dto.Time, editedStat.Time);
            Assert.Equal(dto.Reps, editedStat.Reps);
            Assert.Equal(dto.Weight, editedStat.Weight);
            Assert.Equal(dto.Note, editedStat.Note);
        }

        [Fact]
        public async Task Should_Throw_When_Stat_Does_Not_Exist()
        {
            // Arrange
            using StatDbContextFactory factory = new();
            using IStatDbContext dbContext = await factory.CreateContext();
            CreateEditStatDto dto = _fixture.Create<CreateEditStatDto>();

            // Act / Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => new StatService(dbContext).EditStat(1, 1, dto));
        }

        [Fact]
        public async Task Should_Throw_When_User_Is_Not_Owner_Of_Stat()
        {
            // Arrange
            using StatDbContextFactory factory = new();

            using IStatDbContext arrangeDbContext = await factory.CreateContext();
            Stat stat = (await arrangeDbContext.Stats.AddAsync(_fixture.Create<Stat>())).Entity;
            await arrangeDbContext.SaveChangesAsync();

            CreateEditStatDto dto = _fixture.Create<CreateEditStatDto>();

            using IStatDbContext dbContext = await factory.CreateContext();

            // Act / Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => new StatService(dbContext).EditStat(stat.UserId + 1, stat.Id, dto));
        }
    }
}
