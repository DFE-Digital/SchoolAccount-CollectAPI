using SchoolAccount.Collect.Application.Abstractions.Messaging;
using SchoolAccount.Collect.SharedKernel;

namespace SchoolAccount.Collect.Application.Census.GetCensusActions;

public class GetCensusActionsHandler(ICensusReturnStatusReader returnStatusReader)
    : IQueryHandler<GetCensusActionsQuery, CensusActionsResponse>
{
    public async Task<Result<CensusActionsResponse>> Handle(
        GetCensusActionsQuery query,
        CancellationToken cancellationToken
    )
    {
        if (query.Request.UserDetails.OrgDetails is null)
        {
            throw new ArgumentException("No organisation has been provided.");
        }

        StatusCode? status = await returnStatusReader.GetReturnStatusCode(
            query.Request.UserDetails.OrgDetails[0].Laestab,
            cancellationToken
        );
        CensusActionsResponse response = StubbedCensusResponse.Create(status);

        return await Task.FromResult(Result.Success(response));
    }
}
