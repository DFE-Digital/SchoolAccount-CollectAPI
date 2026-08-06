using System.Text.Json.Serialization;
using SchoolAccount.Web.Api.Infrastructure;

namespace SchoolAccount.Web.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddValidation();
        ReturnEnumsAsString(services);
        return services;
    }

    private static void ReturnEnumsAsString(IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter())
        );
    }
}
