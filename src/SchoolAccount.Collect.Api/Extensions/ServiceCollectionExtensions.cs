using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using SchoolAccount.Collect.Api.Telemetry;

namespace SchoolAccount.Collect.Api.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Sends traces, metrics and logs to the <see cref="TelemetrySettings.Destination"/>.
    /// </summary>
    /// <remarks>
    /// Throws if the destination isn't configured, except OTLP in development.
    /// </remarks>
    public static IServiceCollection AddConfiguredOpenTelemetry(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment
    )
    {
        TelemetryDestination destination = TelemetrySettings.From(configuration).Destination;

        switch (destination)
        {
            case TelemetryDestination.AzureMonitor:
                if (!AzureMonitorSettings.IsConfigured(configuration))
                {
                    throw new InvalidOperationException(
                        $"{TelemetrySettings.SectionName}:{nameof(TelemetrySettings.Destination)} is "
                            + $"{destination}, but {AzureMonitorSettings.ConnectionStringEnvironmentVariable} "
                            + "is not set."
                    );
                }

                services.AddOpenTelemetry().UseAzureMonitor();
                break;

            case TelemetryDestination.Otlp:
                if (!OpenTelemetrySettings.IsOtlpConfigured(configuration))
                {
                    if (environment.IsDevelopment())
                    {
                        break;
                    }

                    throw new InvalidOperationException(
                        $"{TelemetrySettings.SectionName}:{nameof(TelemetrySettings.Destination)} is "
                            + $"{destination}, but {OpenTelemetrySettings.OtlpEndpointEnvironmentVariable} "
                            + "is not set."
                    );
                }

                services
                    .AddOpenTelemetry()
                    .UseOtlpExporter()
                    .WithLogging(_ => { }, options => options.IncludeFormattedMessage = true)
                    .WithTracing(tracing =>
                        tracing
                            .AddAspNetCoreInstrumentation()
                            .AddHttpClientInstrumentation()
                            .AddSqlClientInstrumentation()
                    )
                    .WithMetrics(metrics =>
                        metrics.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation()
                    );

                // Stops Serilog exporting every log twice over http/protobuf.
                services.AddHttpClient("OtlpLogExporter").RemoveAllLoggers();
                break;

            case TelemetryDestination.None:
            default:
                break;
        }

        return services;
    }
}
