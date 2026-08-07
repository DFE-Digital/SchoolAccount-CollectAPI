using System.ComponentModel;

namespace SchoolAccount.Collect.Application.Organisations.GetByLaestab;

public enum OrgStatus
{
    Open = 0,
    Closed = 1,
}

public sealed record OrganisationResponse
{
    [Description("The three digit local authority code")]
    public string LocalAuthorityCode { get; init; }

    [Description("The four digit establishment number")]
    public string EstablishmentNo { get; init; }

    [Description("The status of the organisation")]
    public OrgStatus Status { get; init; }
}
