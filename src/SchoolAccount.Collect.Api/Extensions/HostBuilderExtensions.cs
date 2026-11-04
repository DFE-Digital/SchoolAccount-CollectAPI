using Serilog;

namespace SchoolAccount.Collect.Api.Extensions;

public static class HostBuilderExtensions
{
    /// <summary>
    /// Takes Serilog's sinks and levels from configuration rather than code, so they can be set
    /// per environment, and reads enrichers registered in the container.
    /// </summary>
    /// <remarks>
    /// Also writes to the registered logger providers, so events reach OpenTelemetry.
    /// </remarks>
    public static IHostBuilder UseConfiguredSerilog(this IHostBuilder host) =>
        host.UseSerilog(
            (context, services, loggerConfiguration) =>
                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext(),
            writeToProviders: true
        );
}
