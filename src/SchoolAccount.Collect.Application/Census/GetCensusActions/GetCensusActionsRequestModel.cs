using SchoolAccount.Collect.Application.Shared;

namespace SchoolAccount.Collect.Application.Census.GetCensusActions;

public class GetCensusActionsRequestModel
{
    public required string CensusId { get; init; }
    public required UserDetails UserDetails { get; init; }
}
