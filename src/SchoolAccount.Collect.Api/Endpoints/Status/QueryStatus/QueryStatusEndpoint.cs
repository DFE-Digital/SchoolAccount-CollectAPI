using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Collect.Api.Extensions;
using SchoolAccount.Collect.Api.Infrastructure;
using SchoolAccount.Collect.Application.Abstractions.Messaging;
using SchoolAccount.Collect.Application.Status.QueryStatus;
using SchoolAccount.Collect.SharedKernel;

namespace SchoolAccount.Collect.Api.Endpoints.Status.QueryStatus;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
internal sealed class QueryStatusEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "status",
                async (
                    [FromBody] QueryStatusRequest request,
                    IQueryHandler<QueryStatusQuery, StatusResponse> handler,
                    CancellationToken cancellationToken
                ) =>
                {
                    var orgDetails = request
                        .Organisations.Select(x => new OrgDetails
                        {
                            LocalAuthorityCode = x.LocalAuthorityCode,
                            EstablishmentNo = x.EstablishmentNo,
                        })
                        .ToList();

                    var requestModel = new QueryStatusRequestModel { OrgDetails = orgDetails };

                    var query = new QueryStatusQuery(requestModel);

                    Result<StatusResponse> result = await handler.Handle(query, cancellationToken);

                    return result.Match(Results.Ok, CustomResults.Problem);
                }
            )
            .WithName("QueryStatus")
            .WithSummary("Get status of the service")
            .WithDescription("Retrieves the status of the service")
            .Produces<StatusResponse>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(Tags.Status);
    }
}
