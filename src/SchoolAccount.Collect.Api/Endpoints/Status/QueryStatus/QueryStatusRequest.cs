namespace SchoolAccount.Collect.Api.Endpoints.Status.QueryStatus;

public class QueryStatusRequest
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required List<Organisation> Organisations { get; init; }
}

public class Organisation
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string CategoryId { get; init; }
    public required string LocalAuthorityCode { get; init; }
    public required string EstablishmentNo { get; init; }
}
