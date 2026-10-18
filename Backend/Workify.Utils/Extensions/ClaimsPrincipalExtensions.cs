using System.Security.Claims;

using Workify.Utils.Config;

namespace Workify.Utils.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        extension(ClaimsPrincipal principal)
        {
            public int GetUserId()
            {
                return principal.TryGetUserId() ?? throw new ArgumentException();
            }

            public int? TryGetUserId()
            {
                if (principal.Identity is not ClaimsIdentity identity)
                    return null;

                Claim? claim = identity.FindFirst(CommonConfig.JwtClaimUserId);
                if (claim == null)
                    return null;

                return int.Parse(claim.Value);
            }
        }
    }
}