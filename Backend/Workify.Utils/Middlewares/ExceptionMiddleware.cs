using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Workify.Utils.Extensions;

namespace Workify.Utils.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            HttpRequest request = context.Request;
            request.EnableBuffering();

            string requestBody = string.Empty;
            if (request.ContentLength > 0 && request.Body.CanSeek)
            {
                request.Body.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(request.Body, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                request.Body.Seek(0, SeekOrigin.Begin);
            }

            int? userId;
            try
            {
                userId = context.User.GetUserId();
            }
            catch (Exception)
            {
                userId = null;
            }

            try
            {
                await _next(context);

                var logData = new
                {
                    Method = request.Method,
                    Path = request.Path,
                    Query = request.QueryString.ToString(),
                    RequestBody = requestBody,
                    StatusCode = context.Response.StatusCode,
                    CorrelationId = context.TraceIdentifier,
                    UserId = userId
                };

                _logger.LogInformation("Successful request {@LogData}", logData);
            }
            catch (ArgumentException ex)
            {
                await LogAndHandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest, requestBody, userId);
            }
            catch (Exception ex)
            {
                await LogAndHandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError, requestBody, userId);
            }
        }

        private async Task LogAndHandleExceptionAsync(HttpContext context, Exception exception, int statusCode, string requestBody, int? userId)
        {
            HttpRequest request = context.Request;

            var logData = new
            {
                Scheme = request.Scheme,
                Host = request.Host.ToString(),
                Path = request.Path,
                QueryString = request.QueryString.ToString(),
                Method = request.Method,
                RequestBody = requestBody,
                StatusCode = statusCode,
                ExceptionMessage = exception.Message,
                Exception = exception.ToString(),
                CorrelationId = context.TraceIdentifier,
                UserId = userId
            };

            _logger.LogError(exception, "Exception occurred {@LogData}", logData);

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
