using SchoolAccount.Collect.Application.Abstractions.Messaging;

namespace SchoolAccount.Collect.Application.Status.QueryStatus;

public sealed record QueryStatusQuery(QueryStatusRequestModel request) : IQuery<StatusResponse>;
