namespace SchoolAccount.Collect.Application.Census.GetCensusActions.GetCensusImportantDates;

public interface IGetCensusImportantDates
{
    Task<IEnumerable<ImportantDate>> GetImportantDates(CancellationToken cancellationToken);
}
