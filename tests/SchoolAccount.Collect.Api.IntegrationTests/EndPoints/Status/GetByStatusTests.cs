using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using NSubstitute;
using SchoolAccount.Collect.Api.Endpoints.Status.GetStatus;
using SchoolAccount.Collect.Application.Abstractions.Messaging;
using SchoolAccount.Collect.Application.Status.GetStatus;
using SchoolAccount.Collect.SharedKernel;
using Shouldly;
using Action = SchoolAccount.Collect.Application.Status.GetStatus.Action;

namespace SchoolAccount.Collect.Api.IntegrationTests.EndPoints.Status;

public class GetByStatusTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    private readonly IQueryHandler<GetStatusQuery, StatusResponse> _handler = Substitute.For<
        IQueryHandler<GetStatusQuery, StatusResponse>
    >();

    public GetByStatusTests(WebApplicationFactory<Program> factory)
    {
        _client = factory
            .WithWebHostBuilder(builder =>
                builder.ConfigureTestServices(services =>
                    services.AddScoped<IQueryHandler<GetStatusQuery, StatusResponse>>(_ => _handler)
                )
            )
            .CreateClient();
    }

    [Fact]
    public async Task Status_endpoint_should_return_status_response_with_required_parameters_for_valid_trust_school_input()
    {
        // Arrange
        string id = "test-user-id";
        string email = "test.user@email.com";
        var organisation = new Organisation
        {
            Id = "test-organisation-id",
            Name = "test-organisation-name",
            Category = new Category { Id = "test-category-id", Name = "test-category-name" },
            Ukprn = "test-ukprn",
        };

        var request = new GetStatusRequest
        {
            Id = id,
            Email = email,
            Organisations = new List<Organisation>() { organisation },
        };

        var stubbedStatusResponse = new StatusResponse
        {
            Details = new List<OrganisationResponse>()
            {
                new OrganisationResponse
                {
                    Id = organisation.Id,
                    Name = organisation.Name,
                    CategoryId = organisation.Category.Id,
                    Ukprn = organisation.Ukprn,
                    Laestab = string.Empty,
                    Interesting = false,
                },
            },
        };

        _handler
            .Handle(Arg.Any<GetStatusQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success(stubbedStatusResponse));

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/status",
            request,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        StatusResponse result = await response.Content.ReadFromJsonAsync<StatusResponse>(
            CancellationToken.None
        );

        result.ShouldNotBeNull();

        OrganisationResponse organisationResult = result.Details[0];
        result.Details.Count.ShouldBe(1);
        organisationResult.Id.ShouldBe(stubbedStatusResponse.Details[0].Id);
        organisationResult.Name.ShouldBe(stubbedStatusResponse.Details[0].Name);
        organisationResult.CategoryId.ShouldBe(stubbedStatusResponse.Details[0].CategoryId);
        organisationResult.Ukprn.ShouldBe(stubbedStatusResponse.Details[0].Ukprn);
        organisationResult.Interesting.ShouldBe(false);
        organisationResult.Laestab.ShouldBeNullOrEmpty();
        organisationResult.Actions?.Count.ShouldBe(0);
    }

    [Fact]
    public async Task Status_endpoint_should_return_status_response_with_all_parameters_for_valid_academy_input()
    {
        string id = "test-user-id";
        string email = "test.user@email.com";
        var organisation = new Organisation
        {
            Id = "test-organisation-id",
            Name = "test-organisation-name",
            Category = new Category { Id = "test-category-id", Name = "test-category-name" },
            Ukprn = "test-ukprn",
            LocalAuthority = new LocalAuthority()
            {
                Id = "test-local-authority-id",
                Name = "test-local-authority-name",
                Code = "123",
            },
            EstablishmentNumber = "4567",
        };

        var request = new GetStatusRequest
        {
            Id = id,
            Email = email,
            Organisations = new List<Organisation>() { organisation },
        };

        var stubbedStatusResponse = new StatusResponse
        {
            Details = new List<OrganisationResponse>()
            {
                new OrganisationResponse
                {
                    Id = organisation.Id,
                    Name = organisation.Name,
                    CategoryId = organisation.Category.Id,
                    Ukprn = organisation.Ukprn,
                    Laestab = organisation.LocalAuthority.Code + organisation.EstablishmentNumber,
                    Interesting = true,
                    Actions = new List<Action>() { new Action() },
                },
            },
        };

        _handler
            .Handle(Arg.Any<GetStatusQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success(stubbedStatusResponse));

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/status",
            request,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        StatusResponse result = await response.Content.ReadFromJsonAsync<StatusResponse>(
            CancellationToken.None
        );

        result.ShouldNotBeNull();

        OrganisationResponse organisationResult = result.Details[0];
        organisationResult.Id.ShouldBe(stubbedStatusResponse.Details[0].Id);
        organisationResult.Name.ShouldBe(stubbedStatusResponse.Details[0].Name);
        organisationResult.CategoryId.ShouldBe(stubbedStatusResponse.Details[0].CategoryId);
        organisationResult.Ukprn.ShouldBe(stubbedStatusResponse.Details[0].Ukprn);
        organisationResult.Laestab.ShouldBe(stubbedStatusResponse.Details[0].Laestab);
        organisationResult.Actions.ShouldNotBeNull();
    }

    [Fact]
    public async Task Status_endpoint_should_return_error_messages_for_all_missing_required_parameters_with_null_organisation()
    {
        // Arrange
        var request = new GetStatusRequest
        {
            Id = string.Empty,
            Email = string.Empty,
            Organisations = new List<Organisation>() { new Organisation { } },
        };

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/status",
            request,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        ValidationProblemDetails? validationProblemDetails =
            await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(
                CancellationToken.None
            );
        validationProblemDetails.ShouldNotBeNull();
        validationProblemDetails.Title.ShouldBe("One or more validation errors occurred.");

        validationProblemDetails.Errors.Count.ShouldBe(6);
        validationProblemDetails.Errors.ShouldContainKey("Id");
        validationProblemDetails.Errors.ShouldContainKey("Email");
        validationProblemDetails.Errors.ShouldContainKey("Organisations[0].Id");
        validationProblemDetails.Errors.ShouldContainKey("Organisations[0].Name");
        validationProblemDetails.Errors.ShouldContainKey("Organisations[0].Category");
        validationProblemDetails.Errors.ShouldContainKey("Organisations[0].Ukprn");
        validationProblemDetails.Errors["Id"].ShouldContain("The Id field is required.");
        validationProblemDetails.Errors["Email"].ShouldContain("The Email field is required.");
        validationProblemDetails
            .Errors["Organisations[0].Id"]
            .ShouldContain("The Id field is required.");
        validationProblemDetails
            .Errors["Organisations[0].Name"]
            .ShouldContain("The Name field is required.");
        validationProblemDetails
            .Errors["Organisations[0].Category"]
            .ShouldContain("The Category field is required.");
        validationProblemDetails
            .Errors["Organisations[0].Ukprn"]
            .ShouldContain("The Ukprn field is required.");
    }

    [Fact]
    public async Task Status_endpoint_should_return_error_messages_for_all_missing_required_parameters_with_empty_trust_organisation()
    {
        // Arrange
        var organisation = new Organisation
        {
            Id = string.Empty,
            Name = string.Empty,
            Category = new Category { Id = string.Empty, Name = string.Empty },
            Ukprn = string.Empty,
        };

        var request = new GetStatusRequest
        {
            Id = string.Empty,
            Email = string.Empty,
            Organisations = new List<Organisation>() { organisation },
        };

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/status",
            request,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        ValidationProblemDetails? validationProblemDetails =
            await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(
                CancellationToken.None
            );
        validationProblemDetails.ShouldNotBeNull();
        validationProblemDetails.Title.ShouldBe("One or more validation errors occurred.");

        validationProblemDetails.Errors.Count.ShouldBe(7);
        validationProblemDetails.Errors.ShouldContainKey("Id");
        validationProblemDetails.Errors.ShouldContainKey("Email");
        validationProblemDetails.Errors.ShouldContainKey("Organisations[0].Id");
        validationProblemDetails.Errors.ShouldContainKey("Organisations[0].Name");
        validationProblemDetails.Errors.ShouldContainKey("Organisations[0].Category.Id");
        validationProblemDetails.Errors.ShouldContainKey("Organisations[0].Category.Name");
        validationProblemDetails.Errors.ShouldContainKey("Organisations[0].Ukprn");
        validationProblemDetails.Errors["Id"].ShouldContain("The Id field is required.");
        validationProblemDetails.Errors["Email"].ShouldContain("The Email field is required.");
        validationProblemDetails
            .Errors["Organisations[0].Id"]
            .ShouldContain("The Id field is required.");
        validationProblemDetails
            .Errors["Organisations[0].Name"]
            .ShouldContain("The Name field is required.");
        validationProblemDetails
            .Errors["Organisations[0].Category.Id"]
            .ShouldContain("The Id field is required.");
        validationProblemDetails
            .Errors["Organisations[0].Category.Name"]
            .ShouldContain("The Name field is required.");
        validationProblemDetails
            .Errors["Organisations[0].Ukprn"]
            .ShouldContain("The Ukprn field is required.");
    }

    [Fact]
    public async Task Status_endpoint_should_return_error_messages_for_all_missing_required_parameters_with_empty_academy()
    {
        // Arrange
        var organisation = new Organisation
        {
            Id = string.Empty,
            Name = string.Empty,
            Category = new Category { Id = string.Empty, Name = string.Empty },
            Ukprn = string.Empty,
            LocalAuthority = new LocalAuthority()
            {
                Id = string.Empty,
                Name = string.Empty,
                Code = string.Empty,
            },
            EstablishmentNumber = string.Empty,
        };

        var request = new GetStatusRequest
        {
            Id = string.Empty,
            Email = string.Empty,
            Organisations = new List<Organisation>() { organisation },
        };

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/status",
            request,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        ValidationProblemDetails? validationProblemDetails =
            await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(
                CancellationToken.None
            );
        validationProblemDetails.ShouldNotBeNull();
        validationProblemDetails.Title.ShouldBe("One or more validation errors occurred.");

        validationProblemDetails.Errors.Count.ShouldBe(10);
        validationProblemDetails.Errors.ShouldContainKey("Id");
        validationProblemDetails.Errors.ShouldContainKey("Email");
        validationProblemDetails.Errors.ShouldContainKey("Organisations[0].Id");
        validationProblemDetails.Errors.ShouldContainKey("Organisations[0].Name");
        validationProblemDetails.Errors.ShouldContainKey("Organisations[0].Category.Id");
        validationProblemDetails.Errors.ShouldContainKey("Organisations[0].Category.Name");
        validationProblemDetails.Errors.ShouldContainKey("Organisations[0].Ukprn");
        validationProblemDetails.Errors.ShouldContainKey("Organisations[0].LocalAuthority.Id");
        validationProblemDetails.Errors.ShouldContainKey("Organisations[0].LocalAuthority.Name");
        validationProblemDetails.Errors.ShouldContainKey("Organisations[0].LocalAuthority.Code");
        validationProblemDetails.Errors["Id"].ShouldContain("The Id field is required.");
        validationProblemDetails.Errors["Email"].ShouldContain("The Email field is required.");
        validationProblemDetails
            .Errors["Organisations[0].Id"]
            .ShouldContain("The Id field is required.");
        validationProblemDetails
            .Errors["Organisations[0].Name"]
            .ShouldContain("The Name field is required.");
        validationProblemDetails
            .Errors["Organisations[0].Category.Id"]
            .ShouldContain("The Id field is required.");
        validationProblemDetails
            .Errors["Organisations[0].Category.Name"]
            .ShouldContain("The Name field is required.");
        validationProblemDetails
            .Errors["Organisations[0].Ukprn"]
            .ShouldContain("The Ukprn field is required.");
        validationProblemDetails
            .Errors["Organisations[0].LocalAuthority.Id"]
            .ShouldContain("The Id field is required.");
        validationProblemDetails
            .Errors["Organisations[0].LocalAuthority.Name"]
            .ShouldContain("The Name field is required.");
        validationProblemDetails
            .Errors["Organisations[0].LocalAuthority.Code"]
            .ShouldContain("The Code field is required.");
    }
}
