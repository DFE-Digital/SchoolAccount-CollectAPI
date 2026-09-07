using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SchoolAccount.Collect.Application.Abstractions.Messaging;
using SchoolAccount.Collect.Application.Configuration;
using SchoolAccount.Collect.SharedKernel;

namespace SchoolAccount.Collect.Application.Census.GetCensusActions;

public class GetCensusActionsHandler(IOptionsSnapshot<CensusSettings> settings)
    : IQueryHandler<GetCensusActionsQuery, CensusActionsResponse>
{
    private readonly CensusSettings _settings = settings.Value;

    public async Task<Result<CensusActionsResponse>> Handle(
        GetCensusActionsQuery query,
        CancellationToken cancellationToken
    )
    {
        CensusActionsResponse response;

        if (_settings.UseDatabase)
        {
            await using var connection = new SqlConnection(_settings.ConnectionString);
            string sql =
                "SELECT ReturnStatusCode FROM CollectStateLedger.dbo.CollectReturnStatus WHERE LAEStab = @laestab";
            StatusCode status = await connection.ExecuteScalarAsync<StatusCode>(
                sql,
                new { laestab = query.Request.UserDetails.OrgDetails[0].Laestab }
            );
            response = StubbedCensusResponse.Create(status);
        }
        else
        {
            response = StubbedCensusResponse.Create();
        }

        return await Task.FromResult(Result.Success(response));
    }
}
