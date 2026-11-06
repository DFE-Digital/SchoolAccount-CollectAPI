using Microsoft.Extensions.Options;
using SchoolAccount.Collect.Application.Configuration;
using Shouldly;

namespace SchoolAccount.Collect.Application.UnitTests.Configuration;

public class CensusSettingsValidatorTests
{
    private readonly CensusSettingsValidator _sut = new();

    [Fact]
    public void Failure_is_returned_when_using_the_database_with_an_empty_connection_string()
    {
        // Act
        ValidateOptionsResult result = _sut.Validate("name", CreateSettings(true, string.Empty));

        // Assert
        result.Failed.ShouldBeTrue();
        result.FailureMessage.ShouldBe(
            "The connection string must be provided when using the database."
        );
    }

    [Fact]
    public void Success_is_returned_when_databased_is_not_use_and_a_connection_string_is_provided()
    {
        // Act
        ValidateOptionsResult result = _sut.Validate(
            "name",
            CreateSettings(false, "test-connection-string")
        );

        // Assert
        result.Succeeded.ShouldBeTrue();
    }

    [Fact]
    public void Success_is_returned_when_database_is_used_with_a_connection_string()
    {
        // Act
        ValidateOptionsResult result = _sut.Validate(
            "name",
            CreateSettings(true, "test-connection-string")
        );

        // Assert
        result.Succeeded.ShouldBeTrue();
    }

    private static CensusSettings CreateSettings(bool useDatabase, string? connectionString)
    {
        return new CensusSettings
        {
            UseDatabase = useDatabase,
            ConnectionString = connectionString,
        };
    }
}
