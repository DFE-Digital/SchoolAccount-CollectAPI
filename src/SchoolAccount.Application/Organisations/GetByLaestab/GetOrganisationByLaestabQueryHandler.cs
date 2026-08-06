using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.SharedKernel;

namespace SchoolAccount.Application.Organisations.GetByLaestab;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public sealed class GetOrganisationByLaestabQueryHandler(IDateTimeProvider dateTimeProvider)
    : IQueryHandler<GetOrganisationByLaestabQuery, OrganisationResponse>
{
    public async Task<Result<OrganisationResponse>> Handle(
        GetOrganisationByLaestabQuery query,
        CancellationToken cancellationToken
    )
    {
        var laestabValue = new LaestabValue(query.laestab);
        var statusCalculator = new StatusCalculator(dateTimeProvider);

        var response = new OrganisationResponse
        {
            LocalAuthorityCode = laestabValue.LocalAuthorityCode,
            EstablishmentNo = laestabValue.EstablishmentNo,
            Status = statusCalculator.GetOpenStatus(),
        };

        return await Task.FromResult(Result.Success(response));
    }
}
