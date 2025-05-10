using System.Security.Claims;

using Workify.Utils.Config;

namespace Workify.Utils.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            if (principal.Identity is not ClaimsIdentity identity)
                throw new ArgumentException(nameof(identity));

            Claim? claim = identity.FindFirst(CommonConfig.JwtClaimUserId);
            if (claim == null)
                throw new ArgumentException(nameof(claim));

            return int.Parse(claim.Value);
        }
    }
}