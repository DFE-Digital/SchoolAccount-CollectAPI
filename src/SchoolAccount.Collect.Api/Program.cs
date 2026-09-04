using System.Reflection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;
using SchoolAccount.Collect.Api;
using SchoolAccount.Collect.Api.Extensions;
using SchoolAccount.Collect.Application;
using SchoolAccount.Collect.Infrastructure;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration)
);

if (builder.Environment.IsProduction())
{
    builder.Configuration.AddAzureAppConfiguration();
}

builder.Services.AddApplication(builder.Configuration).AddPresentation().AddInfrastructure();
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

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

await app.RunAsync();
