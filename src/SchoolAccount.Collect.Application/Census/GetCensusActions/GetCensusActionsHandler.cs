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
        if (_settings.UseDatabase)
        {
            throw new NotImplementedException(
                "Reading census data from a database is not supported."
            );
        }

        CensusActionsResponse response = StubbedCensusResponse.Create();

        return await Task.FromResult(Result.Success(response));
    }
}
