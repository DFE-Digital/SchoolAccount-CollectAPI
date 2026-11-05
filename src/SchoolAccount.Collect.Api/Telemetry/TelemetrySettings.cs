namespace SchoolAccount.Collect.Api.Telemetry;

/// <summary>
/// Where the API sends its traces, metrics and logs, chosen per environment in appsettings.
/// </summary>
public enum TelemetryDestination
{
    /// <summary>Telemetry is off.</summary>
    None,

    /// <summary>Application Insights, through the Azure Monitor distro.</summary>
    AzureMonitor,

    /// <summary>An OTLP endpoint, such as Rider's OpenTelemetry tool window.</summary>
    Otlp,
}

public sealed class TelemetrySettings
{
    public const string SectionName = "Telemetry";

    public TelemetryDestination Destination { get; init; } = TelemetryDestination.None;

    public static TelemetrySettings From(IConfiguration configuration) =>
        configuration.GetSection(SectionName).Get<TelemetrySettings>() ?? new TelemetrySettings();
}
