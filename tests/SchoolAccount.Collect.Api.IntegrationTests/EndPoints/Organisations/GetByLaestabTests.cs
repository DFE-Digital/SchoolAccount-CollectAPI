using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using NSubstitute;
using SchoolAccount.Collect.Api.Endpoints;
using SchoolAccount.Collect.Application.Abstractions.Messaging;
using SchoolAccount.Collect.Application.Organisations.GetByLaestab;
using SchoolAccount.Collect.SharedKernel;
using Shouldly;

namespace SchoolAccount.Collect.Api.IntegrationTests.EndPoints.Organisations;

public class GetByLaestabTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    private readonly IQueryHandler<
        GetOrganisationByLaestabQuery,
        OrganisationResponse
    > _organisationHandler = Substitute.For<
        IQueryHandler<GetOrganisationByLaestabQuery, OrganisationResponse>
    >();

    public GetByLaestabTests(WebApplicationFactory<Program> factory)
    {
        _client = factory
            .WithWebHostBuilder(builder =>
                builder.ConfigureTestServices(services =>
                    services.AddScoped<
                        IQueryHandler<GetOrganisationByLaestabQuery, OrganisationResponse>
                    >(_ => _organisationHandler)
                )
            )
            .CreateClient();
    }

    [Fact]
    public async Task Organisations_endpoint_should_return_localAuthorityCode_establishmentNo_and_status_as_a_string_for_valid_laestab()
    {
        // Arrange
        string localAuthorityCode = "987";
        string establishmentNo = "6543";
        string laestab = localAuthorityCode + establishmentNo;

        var stubbedOrganisationResponse = new OrganisationResponse
        {
            LocalAuthorityCode = localAuthorityCode,
            EstablishmentNo = establishmentNo,
            Status = OrgStatus.Closed,
        };

        _organisationHandler
            .Handle(Arg.Any<GetOrganisationByLaestabQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success(stubbedOrganisationResponse));

        // Act
        HttpResponseMessage response = await _client.GetAsync(
            $"/organisations/{laestab}",
            CancellationToken.None
        );

        // Assert
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        ClientOrganisationResponse? result =
            await response.Content.ReadFromJsonAsync<ClientOrganisationResponse>(
                CancellationToken.None
            );

        result.ShouldNotBeNull();
        result.EstablishmentNo.ShouldBe(stubbedOrganisationResponse.EstablishmentNo);
        result.LocalAuthorityCode.ShouldBe(stubbedOrganisationResponse.LocalAuthorityCode);
        result.Status.ShouldBe(stubbedOrganisationResponse.Status.ToString());
    }

    [Fact]
    public async Task Organisations_endpoint_should_return_400_error_and_validation_errors_for_invalid_laestab()
    {
        // Arrange
        string invalidLaestab = "98765";

        // Act
        HttpResponseMessage response = await _client.GetAsync(
            $"/organisations/{invalidLaestab}",
            CancellationToken.None
        );

        // Assert
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);

        ValidationProblemDetails? validationProblemDetails =
            await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(
                CancellationToken.None
            );
        validationProblemDetails.ShouldNotBeNull();
        validationProblemDetails.Title.ShouldBe("One or more validation errors occurred.");

        string errorMessage =
            "LAESTAB identifiers are 7 character numeric only values in the format 1234567";
        validationProblemDetails.Errors.ShouldContainKey("Laestab");
        validationProblemDetails.Errors["Laestab"].ShouldContain(errorMessage);
    }
}
