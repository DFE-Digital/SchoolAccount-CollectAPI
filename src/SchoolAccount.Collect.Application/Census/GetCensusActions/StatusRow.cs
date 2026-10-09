namespace SchoolAccount.Collect.Application.Census.GetCensusActions;

public record StatusRow
{
    public string LAEStab { get; init; }
    public int ReturnStatusCode { get; init; }
    public int? Errors { get; init; }
    public int? Queries { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
