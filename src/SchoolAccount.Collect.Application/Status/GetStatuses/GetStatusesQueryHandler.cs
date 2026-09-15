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
        var laestabs = getStatusesQuery.Request.OrgDetails
            .Select(x => x.LocalAuthorityCode + x.EstablishmentNumber)
            .Where(x => !string.IsNullOrEmpty(x))
            .ToList();

        List<StatusRow> statuses = await returnStatusReader.GetReturnStatusCodes(laestabs, cancellationToken);
        StatusResponse response = CreateStatusResponse(getStatusesQuery, statuses);
        return await Task.FromResult(Result.Success(response));
    }

    private static StatusResponse CreateStatusResponse(GetStatusesQuery getStatusesQuery, List<StatusRow> statuses)
    {
        return new StatusResponse
        {
            Details = getStatusesQuery
                .Request.OrgDetails.Select(x => CreateOrganisationResponse(x, statuses))
                .ToList(),
        };
    }

    private static OrganisationResponse CreateOrganisationResponse(OrgDetails orgDetails, List<StatusRow> statuses)
    {
        string laestab = orgDetails.LocalAuthorityCode + orgDetails.EstablishmentNumber;
        int status = statuses.Where(s => s.LAEStab == laestab).Select(s => s.ReturnStatusCode).FirstOrDefault();
        string statusName = ReturnStatusMapper.GetStatusDescription(status);
        bool interesting = statuses.Any(s => s.LAEStab == laestab);
        return new OrganisationResponse
        {
            Id = orgDetails.Id,
            Name = orgDetails.Name,
            CategoryId = orgDetails.CategoryId,
            Ukprn = orgDetails.Ukprn,
            Laestab = laestab,
            Interesting = interesting,
            Actions = interesting
                ?
                [
                    new()
                    {
                        Id = "autumn-school-census",
                        Name = "Autumn School Census",
                        Status = new Status { Name = statusName },
                    }
                ]
                : []
        };
    }
}
