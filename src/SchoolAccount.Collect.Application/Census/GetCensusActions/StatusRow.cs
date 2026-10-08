namespace SchoolAccount.Collect.Application.Census.GetCensusActions;

public class StatusRow
{
    public string LAEStab { get; set; }
    public int ReturnStatusCode { get; set; }
    public int? Errors { get; set; }
    public int? Queries { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
