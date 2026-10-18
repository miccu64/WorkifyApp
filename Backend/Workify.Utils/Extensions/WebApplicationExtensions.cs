using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;
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

            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = TryEnrichWithUserId;
            });

            app.MapControllers();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi().AllowAnonymous();
                app.MapScalarApiReference().AllowAnonymous();
            }
        }

        private static void TryEnrichWithUserId(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            if (httpContext.User.TryGetUserId() is { } userId)
                diagnosticContext.Set("UserId", userId);
        }
    }
}