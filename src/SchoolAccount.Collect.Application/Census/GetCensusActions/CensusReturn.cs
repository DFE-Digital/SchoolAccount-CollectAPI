namespace SchoolAccount.Collect.Application.Census.GetCensusActions;

public class CensusReturn
{
    public string CollectionId { get; set; }
    public string CollectionName { get; set; }
    public List<StatusRow> StatusRows { get; set; } = [];
}
