using SchoolAccount.Collect.Application.Abstractions.Messaging;

namespace SchoolAccount.Collect.Application.Status.GetStatuses;

public sealed record GetStatusesQuery(GetStatusesRequestModel Request) : IQuery<StatusResponse>;
