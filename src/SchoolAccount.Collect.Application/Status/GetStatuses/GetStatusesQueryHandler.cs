using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Collect.Application.Abstractions.Messaging;
using SchoolAccount.Collect.Application.Census.GetCensusActions;
using SchoolAccount.Collect.SharedKernel;

namespace SchoolAccount.Collect.Application.Status.GetStatuses;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public sealed class GetStatusesQueryHandler(ICensusReturnStatusReader returnStatusReader)
    : IQueryHandler<GetStatusesQuery, StatusResponse>
{
    public async Task<Result<StatusResponse>> Handle(
        GetStatusesQuery getStatusesQuery,
        CancellationToken cancellationToken
    )
    {
        var laestabs = getStatusesQuery
            .Request.OrgDetails.Select(x => x.LocalAuthorityCode + x.EstablishmentNumber)
            .Where(x => !string.IsNullOrEmpty(x))
            .ToList();

        CensusReturn censusReturn = await returnStatusReader.GetCensusReturnStatuses(
            laestabs,
            cancellationToken
        );
        var responseBuilder = new StatusResponseBuilder(censusReturn);
        StatusResponse response = responseBuilder.BuildResponse(
            getStatusesQuery.Request.OrgDetails
        );
        return await Task.FromResult(Result.Success(response));
    }
}
