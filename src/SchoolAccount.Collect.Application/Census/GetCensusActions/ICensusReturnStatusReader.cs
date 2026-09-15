namespace SchoolAccount.Collect.Application.Census.GetCensusActions;

public interface ICensusReturnStatusReader
{
    Task<StatusCode?> GetReturnStatusCode(string? laestab, CancellationToken cancellationToken);
    Task<List<StatusRow>> GetReturnStatusCodes(List<string> laestabs, CancellationToken cancellationToken);

}
