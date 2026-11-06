using System.Reflection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;
using SchoolAccount.Collect.Api;
using SchoolAccount.Collect.Api.Extensions;
using SchoolAccount.Collect.Application;
using SchoolAccount.Collect.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddConfiguredAzureAppConfiguration();

builder.Logging.ClearProviders();
builder.Host.UseConfiguredSerilog();

builder
    .Services.AddApplication(builder.Configuration)
    .AddPresentation(builder.Environment, builder.Configuration)
    .AddInfrastructure();

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
WebApplication app = builder.Build();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapHealthChecks(
    "health",
    new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse }
);

app.UseRequestContextLogging();

app.UseExceptionHandler();

await app.RunAsync();
