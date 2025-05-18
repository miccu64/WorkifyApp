using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Workify.Api.Auth.Services;
using Workify.Utils.Communication.Contracts;
using Workify.Utils.Extensions;

namespace Workify.Api.Auth.Controllers
{
    [ApiController]
    [Route("api/user-management")]
    public class UserManagementController(IUserManagementService userManagementService, IPublishEndpoint publishEndpoint) : ControllerBase
    {
        private readonly IUserManagementService _userManagementService = userManagementService;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        [HttpDelete]
        public async Task<int> DeleteAccount()
        {
            int deletedUserId = await _userManagementService.DeleteUser(User.GetUserId());

            DeletedUserContract deletedUserContract = new(deletedUserId);
            await _publishEndpoint.Publish(deletedUserContract);

            return deletedUserId;
        }
    }
}
