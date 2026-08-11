namespace SchoolAccount.Collect.Application.Status.QueryStatus;

public class QueryStatusRequestModel
{
    public required List<OrgDetails> OrgDetails { get; init; } = new();
}

public class OrgDetails
{
    public required string LocalAuthorityCode { get; init; }
    public required string EstablishmentNo { get; init; }
}
