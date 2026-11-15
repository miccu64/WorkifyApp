using System.Diagnostics;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
            {
                throw;
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
                const int statusCode = StatusCodes.Status500InternalServerError;
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
                CorrelationId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier
            };
            logger.Log(logLevel, "Exception occurred {@LogData}", logData);
        }

        private static async Task RespondAsync(
            HttpContext context,
            int statusCode,
            string message)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;

            ProblemDetails response = new()
            {
                Status = statusCode,
                Title = GetTitle(statusCode),
                Detail = message,
                Type = GetProblemType(statusCode),
            };
            await context.Response.WriteAsJsonAsync(response);
        }

        private static string GetTitle(int statusCode) =>
            statusCode switch
            {
                400 => "Bad request",
                404 => "Not found",
                409 => "Conflict",
                500 => "Error",
                _ => "General error"
            };

        private static string GetProblemType(int statusCode) =>
            statusCode switch
            {
                400 => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                404 => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                409 => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
                500 => "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                _ => ""
            };
    }
}