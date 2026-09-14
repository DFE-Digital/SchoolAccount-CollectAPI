using Microsoft.Extensions.Options;

namespace SchoolAccount.Collect.Application.Configuration;

public class CensusSettingsValidator : IValidateOptions<CensusSettings>
{
    public ValidateOptionsResult Validate(string? name, CensusSettings options)
    {
        if (options.UseDatabase && string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            return ValidateOptionsResult.Fail(
                "The connection string must be provided when using the database."
            );
        }

        return ValidateOptionsResult.Success;
    }
}
