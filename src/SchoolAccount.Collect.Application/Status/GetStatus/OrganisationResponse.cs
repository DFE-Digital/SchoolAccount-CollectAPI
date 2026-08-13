namespace SchoolAccount.Collect.Application.Status.GetStatus;

public sealed record OrganisationResponse
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string CategoryId { get; init; }
    public string Ukprn { get; init; }
    public string Laestab { get; init; }
    public bool Interesting { get; init; }
    public List<Action>? Actions { get; init; }
}
