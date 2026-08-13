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
                    var orgDetails = request
                        .Organisations.Select(x => new OrgDetails
                        {
                            Id = x.Id,
                            Name = x.Name,
                            CategoryId = x.Category.Id,
                            Ukprn = x.Ukprn,
                            LocalAuthorityCode = x.LocalAuthority?.Code,
                            EstablishmentNumber = x.EstablishmentNumber,
                        })
                        .ToList();

                    var requestModel = new GetStatusRequestModel { OrgDetails = orgDetails };

                    var query = new GetStatusQuery(requestModel);

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
