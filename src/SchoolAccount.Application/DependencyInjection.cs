using Microsoft.Extensions.DependencyInjection;
using SchoolAccount.Application.Abstractions.Behaviors;
using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
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
