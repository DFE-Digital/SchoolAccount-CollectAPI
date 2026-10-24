namespace SchoolAccount.Collect.Api.Extensions;

public static class AzureAppConfigurationExtensions
{
    private const string EnabledKey = "AzureAppConfiguration:Enabled";

    /// <summary>
    /// Loads settings from Azure App Configuration and registers the services that refresh them,
    /// when <c>AzureAppConfiguration:Enabled</c> is true.
    /// </summary>
    /// <remarks>
    /// Environment variables are added again afterwards, so they still win over App Configuration.
    /// </remarks>
    public static WebApplicationBuilder AddConfiguredAzureAppConfiguration(
        this WebApplicationBuilder builder
    )
    {
        if (builder.Configuration.GetValue<bool>(EnabledKey))
        {
            builder.Configuration.AddAzureAppConfiguration();

            // Last one wins: re-added so environment variables stay above App Configuration.
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddAzureAppConfiguration();
        }

        return builder;
    }
}
