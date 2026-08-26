using SchoolAccount.Collect.Application.Abstractions.Messaging;

namespace SchoolAccount.Collect.Application.Status.GetStatus;

public sealed record GetStatusQuery(GetStatusRequestModel Request) : IQuery<StatusResponse>;
