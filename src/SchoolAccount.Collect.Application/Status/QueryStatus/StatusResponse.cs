using SchoolAccount.Collect.Application.Organisations.GetByLaestab;

namespace SchoolAccount.Collect.Application.Status.QueryStatus;

public sealed record StatusResponse
{
    public List<OrganisationResponse> Details { get; init; } = new();
}

public sealed record OrganisationResponse
{
    public string LocalAuthorityCode { get; init; } = string.Empty;
    public string EstablishmentNo { get; init; } = string.Empty;
    public string Laestab { get; init; } = string.Empty;
}
