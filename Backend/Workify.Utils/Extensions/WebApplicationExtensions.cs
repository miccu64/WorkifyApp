using Microsoft.AspNetCore.Builder;

namespace Workify.Utils.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void CommonApiInitialization(this WebApplication webApplication)
        {
            webApplication.UseAuthentication();
        }
    }
}