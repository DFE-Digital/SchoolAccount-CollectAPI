namespace SchoolAccount.Collect.Application.Status.GetStatuses;

public class GetStatusesRequestModel
{
    public required List<OrgDetails> OrgDetails { get; init; } = new();
}
