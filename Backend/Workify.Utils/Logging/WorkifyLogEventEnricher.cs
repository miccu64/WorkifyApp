using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using Workify.Utils.Extensions;

namespace Workify.Utils.Logging;

public class WorkifyLogEventEnricher(IHttpContextAccessor httpContextAccessor) : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        int? userId = TryGetUserId();
        if (userId.HasValue)
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserId", userId));
    }

    private int? TryGetUserId()
    {
        try
        {
            return httpContextAccessor.HttpContext?.User.GetUserId();
        }
        catch (Exception)
        {
            return null;
        }
    }
}