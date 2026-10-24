using Microsoft.AspNetCore.Mvc.Testing;

namespace SchoolAccount.Collect.Api.IntegrationTests.Common;

public class CollectApiWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTests");
    }
}
