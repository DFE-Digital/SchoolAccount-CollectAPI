using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Collect.Application.Abstractions.Messaging;
using SchoolAccount.Collect.SharedKernel;

namespace SchoolAccount.Collect.Application.Status.QueryStatus;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public sealed class QueryStatusQueryHandler : IQueryHandler<QueryStatusQuery, StatusResponse>
{
    public async Task<Result<StatusResponse>> Handle(
        QueryStatusQuery query,
        CancellationToken cancellationToken
    )
    {
        var response = new StatusResponse
        {
            Details = query
                .request.OrgDetails.Select(x => new OrganisationResponse
                {
                    LocalAuthorityCode = x.LocalAuthorityCode,
                    EstablishmentNo = x.EstablishmentNo,
                    Laestab = x.LocalAuthorityCode + x.EstablishmentNo,
                })
                .ToList(),
        };

        return await Task.FromResult(Result.Success(response));
    }
}
