using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Collect.Application.Abstractions.Messaging;
using SchoolAccount.Collect.SharedKernel;

namespace SchoolAccount.Collect.Application.Status.GetStatus;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public sealed class GetStatusQueryHandler : IQueryHandler<GetStatusQuery, StatusResponse>
{
    public async Task<Result<StatusResponse>> Handle(
        GetStatusQuery getStatusQuery,
        CancellationToken cancellationToken
    )
    {
        StatusResponse response = CreateStatusResponse(getStatusQuery);

        return await Task.FromResult(Result.Success(response));
    }

    private static StatusResponse CreateStatusResponse(GetStatusQuery getStatusQuery)
    {
        return new StatusResponse
        {
            Details = getStatusQuery
                .Request.OrgDetails.Select(x => CreateOrganisationResponse(x))
                .ToList(),
        };
    }

    private static OrganisationResponse CreateOrganisationResponse(OrgDetails orgDetails)
    {
        string laestab = orgDetails.LocalAuthorityCode + orgDetails.EstablishmentNumber;
        bool interesting = !string.IsNullOrEmpty(laestab);
        return new OrganisationResponse
        {
            Id = orgDetails.Id,
            Name = orgDetails.Name,
            CategoryId = orgDetails.CategoryId,
            Ukprn = orgDetails.Ukprn,
            Laestab = laestab,
            Interesting = interesting,
            Actions = interesting
                ? new List<Action>
                {
                    new Action
                    {
                        Name = "Autumn School Census",
                        Status = new Status { Name = "Not Started" },
                    },
                }
                : new(),
        };
    }
}
