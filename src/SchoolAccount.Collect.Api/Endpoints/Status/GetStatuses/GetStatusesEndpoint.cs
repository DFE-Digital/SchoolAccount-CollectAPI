using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Collect.Api.Extensions;
using SchoolAccount.Collect.Api.Infrastructure;
using SchoolAccount.Collect.Application.Abstractions.Messaging;
using SchoolAccount.Collect.Application.Status.GetStatuses;
using SchoolAccount.Collect.SharedKernel;

namespace SchoolAccount.Collect.Api.Endpoints.Status.GetStatuses;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
internal sealed class GetStatusesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
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
    }
}
