using Workify.Api.Auth.Database;
using Workify.Api.Auth.Models.Entities;

namespace Workify.Api.Auth.Services
{
    internal class UserManagementService(IAuthDbContext dbContext) : IUserManagementService
    {
        public async Task<int> DeleteUser(int userId, CancellationToken ct)
        {
            User userToDelete = await dbContext.Users.FindAsync([userId], ct)
                ?? throw new ArgumentException("User with given id does not exist.");

            dbContext.Users.Remove(userToDelete);
            await dbContext.SaveChangesAsync(ct);

            return userToDelete.Id;
        }
    }
}
