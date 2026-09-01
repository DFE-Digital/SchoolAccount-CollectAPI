using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Collect.Api.Extensions;
using SchoolAccount.Collect.Api.Infrastructure;
using SchoolAccount.Collect.Application.Abstractions.Messaging;
using SchoolAccount.Collect.Application.Census.GetCensusActions;
using SchoolAccount.Collect.SharedKernel;

namespace SchoolAccount.Collect.Api.Endpoints.Census.GetCensusActions;

public class GetCensusActionsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "census/{Id}",
                async (
                    [AsParameters] GetCensusActionsRequest request,
                    IQueryHandler<GetCensusActionsQuery, CensusActionsResponse> handler,
                    CancellationToken cancellationToken
                ) =>
                {
                    GetCensusActionsQuery query = request.ToQuery();

                    Result<CensusActionsResponse> result = await handler.Handle(
                        query,
                        cancellationToken
                    );

                    return result.Match(Results.Ok, CustomResults.Problem);
                }
            )
            .WithName("GetCensusActionDetails")
            .WithSummary("Get details of a census action")
            .WithDescription("Retrieves the details of a census action")
            .Produces<CensusActionsResponse>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(Tags.Status);
    }
}
