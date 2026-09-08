using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SchoolAccount.Collect.Application.Census.GetCensusActions;
using SchoolAccount.Collect.Application.Configuration;

namespace SchoolAccount.Collect.Infrastructure.Census;

public class CensusReturnStatusReader(IOptionsSnapshot<CensusSettings> settings)
    : ICensusReturnStatusReader
{
    private readonly CensusSettings _settings = settings.Value;

    public async Task<StatusCode?> GetReturnStatusCode(
        string? laestab,
        CancellationToken cancellationToken
    )
    {
        if (_settings.UseDatabase)
        {
            await using var connection = new SqlConnection(_settings.ConnectionString);
            string sql =
                "SELECT ReturnStatusCode FROM CollectStateLedger.dbo.CollectReturnStatus WHERE LAEStab = @laestab";
            return await connection.ExecuteScalarAsync<StatusCode>(sql, new { laestab });
        }

        return null;
    }
}
