namespace SchoolAccount.Collect.Application.Census.GetCensusActions.GetCensusImportantDates;

public interface IGetCensusImportantDates
{
    Task<CensusImportantDates> GetImportantDates(CancellationToken cancellationToken);
}
