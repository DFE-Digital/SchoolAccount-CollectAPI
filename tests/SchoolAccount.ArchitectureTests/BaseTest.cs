using System.Reflection;

namespace SchoolAccount.ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly =
        typeof(SchoolAccount.Domain.AssemblyMarker).Assembly;
    protected static readonly Assembly ApplicationAssembly =
        typeof(SchoolAccount.Application.DependencyInjection).Assembly;
    protected static readonly Assembly InfrastructureAssembly =
        typeof(SchoolAccount.Infrastructure.DependencyInjection).Assembly;
    protected static readonly Assembly PresentationAssembly =
        typeof(SchoolAccount.Web.Api.DependencyInjection).Assembly;
}
