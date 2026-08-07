using SchoolAccount.Collect.Application.Abstractions.Messaging;

namespace SchoolAccount.Collect.Application.Organisations.GetByLaestab;

public sealed record GetOrganisationByLaestabQuery(string laestab) : IQuery<OrganisationResponse>;
