using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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
                await LogAndHandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest);
            }
            catch (ArgumentException ex)
            {
                await LogAndHandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                await LogAndHandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError);
            }
        }

        private async Task LogAndHandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
        {
            HttpRequest request = context.Request;

            var logData = new
            {
                Scheme = request.Scheme,
                Host = request.Host.ToString(),
                Path = request.Path,
                QueryString = request.QueryString.ToString(),
                Method = request.Method,
                StatusCode = statusCode,
                ExceptionMessage = exception.Message,
                Exception = exception.ToString(),
                CorrelationId = context.TraceIdentifier
            };

            logger.LogError(exception, "Exception occurred {@LogData}", logData);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                status = statusCode,
                message = exception.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}