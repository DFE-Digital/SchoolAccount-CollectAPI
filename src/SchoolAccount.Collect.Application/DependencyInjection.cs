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
        services
            .AddOptions<CensusSettings>()
            .Bind(configuration.GetSection(CensusSettings.SectionName))
            .Validate(
                s => s.UseDatabase && string.IsNullOrWhiteSpace(s.ConnectionString),
                "The connection string must be provided when using the database."
            );

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
