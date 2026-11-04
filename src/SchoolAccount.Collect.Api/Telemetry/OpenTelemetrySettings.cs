namespace SchoolAccount.Collect.Api.Telemetry;

public static class OpenTelemetrySettings
{
    /// <summary>
    /// The OTLP endpoint variable, set by Rider for local runs.
    /// </summary>
    public const string OtlpEndpointEnvironmentVariable = "OTEL_EXPORTER_OTLP_ENDPOINT";

    /// <summary>
    /// True when the OTLP endpoint environment variable has a value.
    /// </summary>
    public static bool IsOtlpConfigured(IConfiguration configuration) =>
        !string.IsNullOrWhiteSpace(configuration[OtlpEndpointEnvironmentVariable]);
}
