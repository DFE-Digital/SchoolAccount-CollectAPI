# Integration Testing

## Overview
The goal of integration testing in the API project is to verify the stack  behaves as expected, ensuring endpoints are correctly routed, requests are validated and the correct response types and codes are returned.

The underlying implementations called by the endpoints are stubbed to simplify the tests and ensure they are performant. This approach is often referred to as [narrow integration testing](https://martinfowler.com/bliki/IntegrationTest.html). 

Narrow integration testing ensures the stack is wired up correctly prior to running slower end-to-end tests that exercise the full stack, including underlying implementations.

## Implementation

While targeted at MVC sites, the [Integration Testing in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0&pivots=xunit) guide provides a good overview of the approach used in this project.

Rather than starting a web server and sending HTTP requests from a separate process the tests leverage the [WebApplicationFactory](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0&pivots=xunit#basic-tests-with-the-default-webapplicationfactory) to host the API in-process by extending the API's [Program.cs](../src/SchoolAccount.Web.Api/Program.cs). 

This allows the tests to send HTTP requests directly to the API without the overhead of starting a separate process allowing tests run faster in an isolated fashion.

The WebApplicationFactory supports narrow integration testing by allowing injected services to be replaced with mocks using the `ConfigureTestServices` method while building the factory. The below snippet demonstrates replacing the `IQuoteService` with a test implementation `TestQuoteService` during test setup.

```csharp
var client = _factory.WithWebHostBuilder(builder =>
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddScoped<IQuoteService, TestQuoteService>();
        });
    })
    .CreateClient();
```

For more advanced scenarios a [Custom WebApplicationFactory](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0&pivots=xunit#customize-webapplicationfactory) may be created to provide common defaults across tests and reduce boilerplate setup.

The [SchoolAccount.Web.Api.IntegrationTests](../tests/SchoolAccount.Web.Api.IntegrationTests) project contains 'narrow' integration tests for the API endpoints. 

Due to the simplicity of the example API the default WebApplicationFactory is sufficient, however the project demonstrates use of `ConfigureTestServices` to replace the handler with a test implementation.

As with unit tests, the integrations tests abide by the suggested [testing standards](./testing-standards.md).