using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolAccount.Collect.Application.Abstractions.Behaviors;
using SchoolAccount.Collect.Application.Abstractions.Messaging;
using SchoolAccount.Collect.Application.Configuration;

namespace SchoolAccount.Collect.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<CensusSettings>(configuration.GetSection(CensusSettings.SectionName));

        services.Scan(scan =>
            scan.FromAssembliesOf(typeof(DependencyInjection))
                .AddClasses(
                    classes => classes.AssignableTo(typeof(IQueryHandler<,>)),
                    publicOnly: false
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                .AddClasses(
                    classes => classes.AssignableTo(typeof(ICommandHandler<>)),
                    publicOnly: false
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                .AddClasses(
                    classes => classes.AssignableTo(typeof(ICommandHandler<,>)),
                    publicOnly: false
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        services.TryDecorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));

        return services;
    }
}
