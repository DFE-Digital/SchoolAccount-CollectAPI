using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Organisations.GetByLaestab;
using SchoolAccount.Collect.Api.Infrastructure;
using SchoolAccount.SharedKernel;
using SchoolAccount.Collect.Api.Extensions;

namespace SchoolAccount.Collect.Api.Endpoints.Organisations.GetByLaestab;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
internal sealed class GetByLaestabEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "organisations/{laestab}",
                async (
                    [AsParameters] GetByLaestabRequest request,
                    IQueryHandler<GetOrganisationByLaestabQuery, OrganisationResponse> handler,
                    CancellationToken cancellationToken
                ) =>
                {
                    var query = new GetOrganisationByLaestabQuery(request.Laestab);

                    Result<OrganisationResponse> result = await handler.Handle(
                        query,
                        cancellationToken
                    );

                    return result.Match(Results.Ok, CustomResults.Problem);
                }
            )
            .WithName("GetByLaestab")
            .WithSummary("Get organisation details")
            .WithDescription(
                "Retrieves the details and open status of an organisation for the provided LAESTAB"
            )
            .Produces<OrganisationResponse>()
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags(Tags.Organisations);
    }
}
