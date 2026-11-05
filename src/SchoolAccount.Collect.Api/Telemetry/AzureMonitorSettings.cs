namespace SchoolAccount.Collect.Api.Telemetry;

public static class AzureMonitorSettings
{
    /// <summary>
    /// Azure Monitor's connection string variable, set from App Configuration when deployed.
    /// </summary>
    public const string ConnectionStringEnvironmentVariable =
        "APPLICATIONINSIGHTS_CONNECTION_STRING";

    /// <summary>
    /// True when the connection string environment variable has a value.
    /// </summary>
    public static bool IsConfigured(IConfiguration configuration) =>
        !string.IsNullOrWhiteSpace(configuration[ConnectionStringEnvironmentVariable]);
}
