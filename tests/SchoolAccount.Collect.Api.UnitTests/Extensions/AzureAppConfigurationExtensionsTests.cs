using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using SchoolAccount.Collect.Api.Extensions;
using Shouldly;

namespace SchoolAccount.Collect.Api.UnitTests.Extensions;

public class AzureAppConfigurationExtensionsTests
{
    private const string _enabledKey = "AzureAppConfiguration:Enabled";

    [Fact]
    public void Does_not_register_the_refresh_services_when_app_configuration_is_disabled()
    {
        // Arrange
        WebApplicationBuilder builder = CreateBuilder(enabled: false);

        // Act
        builder.AddConfiguredAzureAppConfiguration();

        // Assert
        builder.Services.ShouldNotContain(descriptor =>
            descriptor.ServiceType == typeof(IConfigurationRefresherProvider)
        );
    }

    [Fact]
    public void Loads_app_configuration_when_it_is_enabled()
    {
        // Arrange
        WebApplicationBuilder builder = CreateBuilder(enabled: true);

        // Act
        InvalidOperationException exception = Should.Throw<InvalidOperationException>(
            builder.AddConfiguredAzureAppConfiguration
        );

        // Assert
        exception.Message.ShouldContain("AzureAppConfiguration:Endpoint");
    }

    private static WebApplicationBuilder CreateBuilder(bool enabled)
    {
        WebApplicationBuilder builder = WebApplication.CreateEmptyBuilder(
            new WebApplicationOptions()
        );

        builder.Configuration.AddInMemoryCollection(
            new Dictionary<string, string?> { [_enabledKey] = enabled.ToString() }
        );

        return builder;
    }
}
