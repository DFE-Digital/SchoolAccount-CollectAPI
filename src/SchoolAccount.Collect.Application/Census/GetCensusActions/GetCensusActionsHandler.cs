using SchoolAccount.Collect.Application.Abstractions.Messaging;
using SchoolAccount.Collect.SharedKernel;

namespace SchoolAccount.Collect.Application.Census.GetCensusActions;

public class GetCensusActionsHandler : IQueryHandler<GetCensusActionsQuery, CensusActionsResponse>
{
    public async Task<Result<CensusActionsResponse>> Handle(
        GetCensusActionsQuery query,
        CancellationToken cancellationToken
    )
    {
        CensusActionsResponse response = StubbedCensusResponse.Create();

        return await Task.FromResult(Result.Success(response));
    }
}
