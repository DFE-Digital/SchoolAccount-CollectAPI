using Microsoft.Extensions.DependencyInjection;
using SchoolAccount.Infrastructure.Time;
using SchoolAccount.SharedKernel;

namespace SchoolAccount.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddServices().AddHealthChecks();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
