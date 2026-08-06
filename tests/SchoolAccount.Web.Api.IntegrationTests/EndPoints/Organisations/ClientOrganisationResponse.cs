namespace SchoolAccount.Web.Api.IntegrationTests.EndPoints.Organisations;

public record ClientOrganisationResponse(
    string LocalAuthorityCode,
    string EstablishmentNo,
    string Status
);
