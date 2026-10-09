namespace SchoolAccount.Collect.Application.Status.GetStatuses;

public class Action
{
    public string Id { get; init; }
    public string Name { get; init; }
    public Status Status { get; init; }
    public int? Errors { get; init; }
    public int? Queries { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
