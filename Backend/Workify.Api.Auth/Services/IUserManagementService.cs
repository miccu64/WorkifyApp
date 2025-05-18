namespace Workify.Api.Auth.Services
{
    public interface IUserManagementService
    {
        Task<int> DeleteUser(int userId);
    }
}
