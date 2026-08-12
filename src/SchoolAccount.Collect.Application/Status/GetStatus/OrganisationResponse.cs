namespace SchoolAccount.Collect.Application.Status.GetStatus;

public sealed record OrganisationResponse
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string CategoryId { get; init; } = string.Empty;
    public string Ukprn { get; init; } = string.Empty;
    public string Laestab { get; init; } = string.Empty;
    public bool? Interesting { get; init; }
    public List<Action>? Actions { get; init; }
}
