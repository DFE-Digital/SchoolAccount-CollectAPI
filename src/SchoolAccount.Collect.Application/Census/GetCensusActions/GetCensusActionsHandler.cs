using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SchoolAccount.Collect.Application.Abstractions.Messaging;
using SchoolAccount.Collect.Application.Configuration;
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
        StatusCode? status = await returnStatusReader.GetReturnStatusCode(
            query.Request.UserDetails.OrgDetails[0].Laestab,
            cancellationToken
        );
        CensusActionsResponse response = StubbedCensusResponse.Create(status);

        return await Task.FromResult(Result.Success(response));
    }
}
