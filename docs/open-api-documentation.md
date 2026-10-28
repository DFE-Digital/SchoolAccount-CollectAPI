# Open API Documentation

## Overview
The [Minimal API](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-10.0&source=recommendations) approach used in the project includes a number of extension methods that can be used to generate Open API documentation.

The below covers the recommended areas to document when adding a new API endpoint. Full documentation on the available options for both Minimal and Controller based APIs can be found at [Include OpenAPI metadata in an ASP.NET Core app](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/include-metadata?view=aspnetcore-10.0&tabs=minimal-apis).

## Organisation and grouping

The `WithTags` extension method provides a mechanism to categorise operations into related groups. All endpoints with the same tag will be grouped into the same section within the Scalar UI.

The following code ensures the  `GET` and `POST` endpoints will be grouped under the `Todo` section in the Scalar UI:

```csharp
app.MapGet("/todo", GetTodoItem)
    .WithTags("Todo");

app.MapPost("/todo", CreateTodoItem)
    .WithTags("Todo");
```

## Endpoint names, summaries, and descriptions

The following extension methods can be used to provide high level information for an endpoint:

- `WithName` - provides a name for the endpoint, used as the `operationId` within the OpenAPI specification
- `WithSummary` -  to provide a brief summary of the endpoint
- `WithDescription` - provides a more detailed description

The information will be displayed in the Scalar UI and in the Open API documentation.

## Annotating requests

Annotations on request classes can be used to provide descriptions of individual parameters. 

An example can be seen in [GetStatusesRequest](../src/SchoolAccount.Collect.Api/Endpoints/Status/GetStatuses/GetStatusesRequest.cs) which is annotated with a `Required` and a `EmailAddress` annotation.

```csharp
    [Description("LAESTAB of the organisation. Must be a seven digit number.")]
    public string Laestab { get; init; }
```

## Response types and codes

The `Produces` extension method can be used to specify the response type and status code for an endpoint, i.e. `.Produces<StatusResponse>()`.

The specified type can be annotated in the same way as annotating a request in the previous section.

Specific problem responses codes can be documented using the `ProducesProblem` extension method. The method should be used to specify each HTTP response code that can be returned from an endpoint. The below snippet shows an example of documenting that the API may throw a 500 error.

```csharp
    .ProducesProblem(StatusCodes.Status500InternalServerError);
```

Note the use of the named constants on the `StatusCodes` class to avoid 'magic numbers' like 500 in the code. 

For validation errors the specific `ProducesValidationProblem` extenstion method should be used. This will likely be required on all end points.

## Example of documented endpoint

All the recommendations above can be found on the `GetStatuses` endpoint in the [GetStatusesEndpoint](../src/SchoolAccount.Collect.Api/Endpoints/Status/GetStatuses/GetStatusesEndpoint.cs) endpoint.

```csharp
app.MapPost(
                "status",
                async (
                    [FromBody] GetStatusesRequest request,
                    IQueryHandler<GetStatusesQuery, StatusResponse> handler,
                    CancellationToken cancellationToken
                ) =>
                {
                    GetStatusesQuery query = request.ToQuery();

                    Result<StatusResponse> result = await handler.Handle(query, cancellationToken);

                    return result.Match(Results.Ok, CustomResults.Problem);
                }
            )
            .WithName("GetStatus")
            .WithSummary("Get status of the service")
            .WithDescription("Retrieves the status of the service")
            .Produces<StatusResponse>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(Tags.Status);
```
