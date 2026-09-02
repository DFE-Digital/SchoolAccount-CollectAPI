using SchoolAccount.Collect.Application.Status.GetStatuses;

namespace SchoolAccount.Collect.Application.Shared;

public class UserDetails
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required List<OrgDetails> OrgDetails { get; init; } = [];
}
