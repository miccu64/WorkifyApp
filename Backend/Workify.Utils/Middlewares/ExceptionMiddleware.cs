using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Workify.Utils.Exceptions;

namespace Workify.Utils.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                // TODO: centralize FluentVal or implement other validation handling
                const int statusCode = StatusCodes.Status400BadRequest;
                LogException(context, ex, statusCode);
                await RespondAsync(context, statusCode, ex.Message);
            }
            catch (NotFoundException ex)
            {
                const int statusCode = StatusCodes.Status404NotFound;
                LogException(context, ex, statusCode);
                await RespondAsync(context, statusCode, ex.Message);
            }
            catch (ConflictException ex)
            {
                const int statusCode = StatusCodes.Status409Conflict;
                LogException(context, ex, statusCode);
                await RespondAsync(context, statusCode, ex.Message);
            }
            catch (Exception ex)
            {
                const int statusCode = StatusCodes.Status409Conflict;
                LogException(context, ex, statusCode, LogLevel.Error);
                await RespondAsync(context, statusCode, "Internal server error");
            }
        }

        private void LogException(
            HttpContext context,
            Exception exception,
            int statusCode,
            LogLevel logLevel = LogLevel.Information)
        {
            if (!logger.IsEnabled(logLevel))
                return;

            var logData = new
            {
                Scheme = context.Request.Scheme,
                Host = context.Request.Host.ToString(),
                Path = context.Request.Path,
                QueryString = context.Request.QueryString.ToString(),
                Method = context.Request.Method,
                StatusCode = statusCode,
                ExceptionMessage = exception.Message,
                Exception = exception.ToString(),
                CorrelationId = context.TraceIdentifier
            };

            logger.Log(logLevel, "Exception occurred {@LogData}", logData);
        }

        private static async Task RespondAsync(
            HttpContext context,
            int statusCode,
            string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                status = statusCode,
                message = message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}