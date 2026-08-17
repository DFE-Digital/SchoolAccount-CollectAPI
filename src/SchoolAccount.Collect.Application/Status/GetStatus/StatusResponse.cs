namespace SchoolAccount.Collect.Application.Status.GetStatus;

public sealed record StatusResponse
{
    public List<OrganisationResponse> Details { get; init; } = new();
}
