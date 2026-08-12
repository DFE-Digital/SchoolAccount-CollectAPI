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
        var response = new StatusResponse
        {
            Details = getStatusQuery
                .request.OrgDetails.Select(x =>
                {
                    string laestab = x.LocalAuthorityCode + x.EstablishmentNumber;
                    bool interesting = !string.IsNullOrEmpty(laestab);
                    return new OrganisationResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        CategoryId = x.CategoryId,
                        Ukprn = x.Ukprn,
                        Laestab = laestab,
                        Interesting = interesting,
                        Actions = interesting ? new List<Action> { new() } : new(),
                    };
                })
                .ToList(),
        };

        return await Task.FromResult(Result.Success(response));
    }
}
