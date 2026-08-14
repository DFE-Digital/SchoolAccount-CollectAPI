using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Collect.Api.Extensions;
using SchoolAccount.Collect.Api.Infrastructure;
using SchoolAccount.Collect.Application.Abstractions.Messaging;
using SchoolAccount.Collect.Application.Status.GetStatus;
using SchoolAccount.Collect.SharedKernel;

namespace SchoolAccount.Collect.Api.Endpoints.Status.GetStatus;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
internal sealed class GetStatusEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "status",
                async (
                    [FromBody] GetStatusRequest request,
                    IQueryHandler<GetStatusQuery, StatusResponse> handler,
                    CancellationToken cancellationToken
                ) =>
                {
                    GetStatusQuery query = request.ToQuery();

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
