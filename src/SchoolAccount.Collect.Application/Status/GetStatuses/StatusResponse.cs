namespace SchoolAccount.Collect.Application.Status.GetStatuses;

public sealed record StatusResponse
{
    public List<OrganisationResponse> Details { get; init; } = new();
}
