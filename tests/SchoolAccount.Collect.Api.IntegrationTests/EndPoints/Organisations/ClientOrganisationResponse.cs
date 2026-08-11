namespace SchoolAccount.Collect.Api.IntegrationTests.EndPoints.Organisations;

public record ClientOrganisationResponse(
    string LocalAuthorityCode,
    string EstablishmentNumber,
    string Status
);
