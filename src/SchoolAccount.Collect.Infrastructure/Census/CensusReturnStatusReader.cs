using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SchoolAccount.Collect.Application.Census.GetCensusActions;
using SchoolAccount.Collect.Application.Configuration;

namespace SchoolAccount.Collect.Infrastructure.Census;

public class CensusReturnStatusReader(
    IOptionsSnapshot<CensusSettings> settings,
    ILogger<CensusReturnStatusReader> logger
) : ICensusReturnStatusReader
{
    private readonly CensusSettings _settings = settings.Value;

    public async Task<int?> GetReturnStatusCode(string laestab, CancellationToken cancellationToken)
    {
        if (_settings.UseDatabase)
        {
            await using var connection = new SqlConnection(_settings.ConnectionString);
            string sql =
                "SELECT ReturnStatusCode FROM CollectStateLedger.dbo.CollectReturnStatus WHERE LAEStab = @laestab AND Collection = @collection ORDER BY UpdatedAt DESC";

            logger.ExecutingReturnStatusQuery(_settings.CurrentOpenCensus, sql);

            int? returnStatusCode = await connection.ExecuteScalarAsync<int?>(
                new CommandDefinition(
                    sql,
                    new { LAEStab = laestab, Collection = _settings.CurrentOpenCensus },
                    cancellationToken: cancellationToken
                )
            );

            logger.CompletedReturnStatusQuery(returnStatusCode);

            return returnStatusCode;
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

            logger.ExecutingCensusReturnStatusQuery(
                laestabs.Count,
                _settings.CurrentOpenCensus,
                sql
            );

            IEnumerable<StatusRow> result = await connection.QueryAsync<StatusRow>(
                sql,
                queryParams
            );
            censusReturn.StatusRows = result.ToList();

            logger.CompletedCensusReturnStatusQuery(censusReturn.StatusRows.Count);
        }

        return censusReturn;
    }
}
