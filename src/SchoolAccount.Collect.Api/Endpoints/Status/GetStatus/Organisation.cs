namespace SchoolAccount.Collect.Api.Endpoints.Status.GetStatus;

public class Organisation
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required Category Category { get; init; }
    public required string Ukprn { get; init; }
    public LocalAuthority? LocalAuthority { get; init; }
    public string? EstablishmentNumber { get; init; }
}
