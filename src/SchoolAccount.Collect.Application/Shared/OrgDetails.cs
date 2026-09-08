namespace SchoolAccount.Collect.Application.Status.GetStatuses;

public class OrgDetails
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string CategoryId { get; init; }
    public required string Ukprn { get; init; }
    public string? LocalAuthorityCode { get; init; }
    public string? EstablishmentNumber { get; init; }
    public string? Laestab => CreateLaestab();

    private string? CreateLaestab()
    {
        return string.IsNullOrEmpty(LocalAuthorityCode) || string.IsNullOrEmpty(EstablishmentNumber)
            ? null
            : LocalAuthorityCode + EstablishmentNumber;
    }
}
