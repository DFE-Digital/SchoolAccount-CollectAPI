namespace SchoolAccount.Collect.Application.Status.GetStatuses;

public class Action
{
    public string Id { get; init; }
    public string Name { get; init; }
    public Status Status { get; init; }
    public int? Errors { get; set; }
    public int? Queries { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
