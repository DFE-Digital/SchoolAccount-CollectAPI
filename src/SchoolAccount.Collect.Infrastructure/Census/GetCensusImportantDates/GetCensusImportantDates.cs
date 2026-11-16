using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SchoolAccount.Collect.Application.Census.GetCensusActions;
using SchoolAccount.Collect.Application.Census.GetCensusActions.GetCensusImportantDates;
using SchoolAccount.Collect.Application.Configuration;

namespace SchoolAccount.Collect.Infrastructure.Census.GetCensusImportantDates;

public class GetCensusImportantDates(
    IOptionsSnapshot<CensusSettings> settings,
    ILogger<GetCensusImportantDates> logger
) : IGetCensusImportantDates
{
    private readonly CensusSettings _settings = settings.Value;

    public async Task<CensusImportantDates> GetImportantDates(CancellationToken cancellationToken)
    {
        if (_settings.UseDatabase)
        {
            await using var connection = new SqlConnection(_settings.ConnectionString);
            string sql =
                @"
                SELECT DCOpenDate, DCCloseDate, DCDueDate
                FROM COLLECTPortal.dbo.DataCollection
                    WHERE 
                        DCName = @collection
                    ";

            logger.ExecutingReturnStatusQuery(_settings.CurrentOpenCensus, sql);

            CensusImportantDates result = await connection.ExecuteScalarAsync<CensusImportantDates>(
                sql,
                _settings.CurrentOpenCensus
            );

            return result;
        }

        return null;
    }
}
