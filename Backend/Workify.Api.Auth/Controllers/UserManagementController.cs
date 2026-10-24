using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Workify.Api.Auth.Services;
using Workify.Utils.Communication.Contracts;
using Workify.Utils.Extensions;

namespace Workify.Api.Auth.Controllers
{
    [ApiController]
    [Route("api/user-management")]
    public class UserManagementController(
        IUserManagementService userManagementService,
        IPublishEndpoint publishEndpoint,
        CancellationToken ct) : ControllerBase
    {
        [HttpDelete]
        public async Task<int> DeleteAccount(CancellationToken ct)
        {
            int deletedUserId = await userManagementService.DeleteUser(User.GetUserId(), ct);

            DeletedUserContract deletedUserContract = new(deletedUserId);
            await publishEndpoint.Publish(deletedUserContract, ct);

            return deletedUserId;
        }
    }
}