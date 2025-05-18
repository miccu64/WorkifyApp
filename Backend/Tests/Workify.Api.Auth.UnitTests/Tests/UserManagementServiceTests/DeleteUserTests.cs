using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Workify.Api.Auth.Database;
using Workify.Api.Auth.Models.Entities;
using Workify.Api.Auth.Services;
using Workify.Api.Auth.UnitTests.Utils;

namespace Workify.Api.Auth.UnitTests.Tests.UserManagementServiceTests
{
    public class DeleteUserTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task Should_Delete_User()
        {
            // Arrange
            AuthDbContextFactory factory = new();

            using IAuthDbContext arrangeDbContext = await factory.CreateContext();
            List<User> users = _fixture.CreateMany<User>(5).ToList();
            await arrangeDbContext.Users.AddRangeAsync(users);
            await arrangeDbContext.SaveChangesAsync();

            User userToDelete = users[2];

            using IAuthDbContext dbContext = await factory.CreateContext();
            UserManagementService userManagementService = new(dbContext);

            // Act
            int deletedUserId = await userManagementService.DeleteUser(userToDelete.Id);

            // Assert
            Assert.Equal(userToDelete.Id, deletedUserId);

            using IAuthDbContext assertDbContext = await factory.CreateContext();
            Assert.Null(await assertDbContext.Users.FindAsync(deletedUserId));
            Assert.Equal(users.Count - 1, await assertDbContext.Users.CountAsync());
        }

        [Fact]
        public async Task Should_Throw_When_User_Does_Not_Exist()
        {
            // Arrange
            AuthDbContextFactory factory = new();

            using IAuthDbContext dbContext = await factory.CreateContext();
            UserManagementService userManagementService = new(dbContext);

            const int userIdToDelete = 11;

            // Act / Assert
            await Assert.ThrowsAsync<ArgumentException>(() => userManagementService.DeleteUser(userIdToDelete));
        }
    }
}
