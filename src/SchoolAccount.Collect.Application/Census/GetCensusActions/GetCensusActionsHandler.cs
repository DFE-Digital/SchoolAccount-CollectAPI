using SchoolAccount.Collect.Application.Abstractions.Messaging;
using SchoolAccount.Collect.Application.Census.GetCensusActions.GetCensusImportantDates;
using SchoolAccount.Collect.Application.Shared;
using SchoolAccount.Collect.SharedKernel;

namespace SchoolAccount.Collect.Application.Census.GetCensusActions;

public class GetCensusActionsHandler(
    ICensusReturnStatusReader returnStatusReader,
    IGetCensusImportantDates returnImportantDatesReader
) : IQueryHandler<GetCensusActionsQuery, CensusActionsResponse>
{
    public async Task<Result<CensusActionsResponse>> Handle(
        GetCensusActionsQuery query,
        CancellationToken cancellationToken
    )
    {
        OrgDetails firstOrganisation =
            query.Request.UserDetails.OrgDetails.FirstOrDefault()
            ?? throw new ArgumentException("No organisation has been provided.");

        int? status = await returnStatusReader.GetReturnStatusCode(
            firstOrganisation.Laestab,
            cancellationToken
        );

        IEnumerable<ImportantDate> importantDates =
            await returnImportantDatesReader.GetImportantDates(cancellationToken);

        CensusActionsResponse response = StubbedCensusResponse.Create(importantDates, status);

        return await Task.FromResult(Result.Success(response));
    }
}
