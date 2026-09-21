using Microsoft.Extensions.Logging;

namespace SchoolAccount.Collect.Infrastructure.Census;

// Log definitions for the census return status queries, kept out of the reader so the query
// code stays readable. Callers pass their own ILogger, so the log category - and therefore the
// Serilog level override that switches these on - stays with the calling type.
//
// Interim measure. We are moving to OpenTelemetry, and its SqlClient instrumentation tracks the
// SQL command properly - including the statement Dapper actually builds, so list parameters show
// up expanded rather than as "IN @laestabs". Logging at the call site is simpler for now and
// enough to see what is going to the database.
internal static partial class CensusReturnStatusReaderInstrumentation
{
    [LoggerMessage(
        EventId = 4001,
        Level = LogLevel.Debug,
        Message = "Executing return status query for collection {collection}: {sql}"
    )]
    internal static partial void ExecutingReturnStatusQuery(
        this ILogger logger,
        string collection,
        string sql
    );

    [LoggerMessage(
        EventId = 4002,
        Level = LogLevel.Debug,
        Message = "Return status query returned status code {returnStatusCode}"
    )]
    internal static partial void CompletedReturnStatusQuery(
        this ILogger logger,
        int? returnStatusCode
    );

    [LoggerMessage(
        EventId = 4003,
        Level = LogLevel.Debug,
        Message = "Executing census return status query for {laestabCount} laestabs in collection {collection}: {sql}"
    )]
    internal static partial void ExecutingCensusReturnStatusQuery(
        this ILogger logger,
        int laestabCount,
        string collection,
        string sql
    );

    [LoggerMessage(
        EventId = 4004,
        Level = LogLevel.Debug,
        Message = "Census return status query returned {rowCount} rows"
    )]
    internal static partial void CompletedCensusReturnStatusQuery(
        this ILogger logger,
        int rowCount
    );
}
