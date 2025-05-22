using Microsoft.AspNetCore.Builder;
using Serilog;
using Workify.Utils.Middlewares;

namespace Workify.Utils.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void CommonApiInitialization(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseSerilogRequestLogging();

            app.MapControllers();
        }
    }
}
