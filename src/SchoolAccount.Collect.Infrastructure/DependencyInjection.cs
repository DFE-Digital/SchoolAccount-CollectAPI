using Microsoft.Extensions.DependencyInjection;
using SchoolAccount.Collect.Application.Census.GetCensusActions;
using SchoolAccount.Collect.Infrastructure.Census;

namespace SchoolAccount.Collect.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddServices().AddHealthChecks();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<ICensusReturnStatusReader, CensusReturnStatusReader>();

        return services;
    }
}
