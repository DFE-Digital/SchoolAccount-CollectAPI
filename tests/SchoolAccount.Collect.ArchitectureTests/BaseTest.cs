using System.Reflection;
using SchoolAccount.Collect.Application;
using SchoolAccount.Collect.Domain;

namespace SchoolAccount.Collect.ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(AssemblyMarker).Assembly;

    protected static readonly Assembly ApplicationAssembly = typeof(DependencyInjection).Assembly;

    protected static readonly Assembly InfrastructureAssembly =
        typeof(Infrastructure.DependencyInjection).Assembly;

    protected static readonly Assembly PresentationAssembly =
        typeof(Api.DependencyInjection).Assembly;
}
