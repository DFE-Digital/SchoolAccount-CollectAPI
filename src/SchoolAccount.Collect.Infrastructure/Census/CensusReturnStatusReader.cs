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
                "SELECT ReturnStatusCode FROM CollectStateLedger.dbo.CollectReturnStatus WHERE LAEStab = @laestab AND Collection = @collection ORDER BY UpdatedAt DESC";
            return await connection.ExecuteScalarAsync<StatusCode?>(
                new CommandDefinition(
                    sql,
                    new { LAEStab = laestab, Collection = _settings.CurrentOpenCensus },
                    cancellationToken: cancellationToken
                )
            );
        }

        return null;
    }

    public async Task<CensusReturn> GetCensusReturnStatuses(
        List<string> laestabs,
        CancellationToken cancellationToken
    )
    {
        var censusReturn = new CensusReturn
        {
            CollectionName = _settings.CurrentOpenCensusDisplayName,
            CollectionId = _settings.CurrentOpenCensus,
        };

        if (laestabs.Count == 0)
        {
            return censusReturn;
        }

        if (_settings.UseDatabase)
        {
            await using var connection = new SqlConnection(_settings.ConnectionString);
            string sql =
                @"
                SELECT ReturnStatusCode, LAEStab
                FROM (
                    SELECT ReturnStatusCode, LAEStab,ROW_NUMBER() OVER 
                    (PARTITION BY LAEStab, Collection ORDER BY UpdatedAt DESC) AS rn
                    FROM CollectStateLedger.dbo.CollectReturnStatus
                    WHERE 
                        LAEStab IN @laestabs
                        AND Collection = @collection
                    ) ranked
                WHERE rn = 1;";

            var queryParams = new { Laestabs = laestabs, Collection = _settings.CurrentOpenCensus };

            IEnumerable<StatusRow> result = await connection.QueryAsync<StatusRow>(
                sql,
                queryParams
            );
            censusReturn.StatusRows = result.ToList();
        }

        return censusReturn;
    }
}
