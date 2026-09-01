using SchoolAccount.Collect.Application.Abstractions.Messaging;

namespace SchoolAccount.Collect.Application.Census.GetCensusActions;

public sealed record GetCensusActionsQuery(GetCensusActionsRequestModel Request)
    : IQuery<CensusActionsResponse>;
