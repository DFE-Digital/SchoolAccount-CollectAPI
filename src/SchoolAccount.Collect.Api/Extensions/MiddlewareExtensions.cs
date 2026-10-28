using SchoolAccount.Collect.Api.Middleware;
using Serilog;
using Serilog.Events;

namespace SchoolAccount.Collect.Api.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();

        return app;
    }

    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging(options => options.GetLevel = GetRequestLogLevel);

        return app;
    }

    // Decides how loudly to record each request summary.
    //
    // The container platform polls the health endpoint constantly, and a summary line per probe
    // buries everything else in the log stream. Those drop to Verbose, which nothing collects,
    // so they cost nothing but can be switched on if the probes themselves need investigating.
    //
    // Everything else keeps Serilog's own defaults: failures at Error, ordinary requests at
    // Information. The status code is checked as well as the exception because the global
    // exception handler deals with most failures before they reach this middleware, so a 500
    // usually arrives here with no exception attached.
    private static LogEventLevel GetRequestLogLevel(
        HttpContext context,
        double elapsedMilliseconds,
        Exception? exception
    )
    {
        if (exception is not null || context.Response.StatusCode >= 500)
        {
            return LogEventLevel.Error;
        }

        return context.Request.Path.StartsWithSegments("/health")
            ? LogEventLevel.Verbose
            : LogEventLevel.Information;
    }
}
