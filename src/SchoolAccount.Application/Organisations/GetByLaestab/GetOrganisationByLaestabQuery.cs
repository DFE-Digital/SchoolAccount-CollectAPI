using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Organisations.GetByLaestab;

public sealed record GetOrganisationByLaestabQuery(string laestab) : IQuery<OrganisationResponse>;
