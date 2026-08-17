namespace SchoolAccount.Collect.Application.Status.GetStatus;

public class GetStatusRequestModel
{
    public required List<OrgDetails> OrgDetails { get; init; } = new();
}
