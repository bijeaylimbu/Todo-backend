namespace TodoApi.BuildingBlocks.Logging;

public static class HttpContextCorrelationExtensions
{
    public static string CorrelationHeader(this HttpContext httpContext)
    {
        httpContext.Request.Headers.TryGetValue(LogConstants.CorrelationName, out var source);
        return source.FirstOrDefault() ?? httpContext.TraceIdentifier;
    }
}