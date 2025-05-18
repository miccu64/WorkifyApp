using Workify.Api.Auth.Database;
using Workify.Api.Auth.Models.Entities;

namespace Workify.Api.Auth.Services
{
    internal class UserManagementService(IAuthDbContext dbContext) : IUserManagementService
    {
        private readonly IAuthDbContext _dbContext = dbContext;

        public async Task<int> DeleteUser(int userId)
        {
            User? userToDelete = await _dbContext.Users.FindAsync(userId)
                ?? throw new ArgumentException("User with given id does not exist.");

            _dbContext.Users.Remove(userToDelete);
            await _dbContext.SaveChangesAsync();

            return userToDelete.Id;
        }
    }
}
