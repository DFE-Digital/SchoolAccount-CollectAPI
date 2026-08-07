using System.Reflection;

namespace SchoolAccount.Collect.ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(Domain.AssemblyMarker).Assembly;
    protected static readonly Assembly ApplicationAssembly =
        typeof(Application.DependencyInjection).Assembly;
    protected static readonly Assembly InfrastructureAssembly =
        typeof(Infrastructure.DependencyInjection).Assembly;
    protected static readonly Assembly PresentationAssembly =
        typeof(Api.DependencyInjection).Assembly;
}
