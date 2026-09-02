using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using NSubstitute;
using SchoolAccount.Collect.Api.Endpoints.Shared;
using SchoolAccount.Collect.Api.Endpoints.Status.GetStatuses;
using SchoolAccount.Collect.Application.Abstractions.Messaging;
using SchoolAccount.Collect.Application.Census.GetCensusActions;
using SchoolAccount.Collect.SharedKernel;
using Shouldly;

namespace SchoolAccount.Collect.Api.IntegrationTests.EndPoints.Census;

public class GetCensusActionsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string _censusId = "autumn-school-census-2026";

    private readonly HttpClient _client;

    private readonly IQueryHandler<GetCensusActionsQuery, CensusActionsResponse> _handler =
        Substitute.For<IQueryHandler<GetCensusActionsQuery, CensusActionsResponse>>();

    public GetCensusActionsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory
            .WithWebHostBuilder(builder =>
                builder.ConfigureTestServices(services =>
                    services.AddScoped<IQueryHandler<GetCensusActionsQuery, CensusActionsResponse>>(
                        _ => _handler
                    )
                )
            )
            .CreateClient();
    }

    [Fact]
    public async Task CensusActions_endpoint_should_return_census_actions_response()
    {
        // Arrange
        User user = ValidUser();

        CensusActionsResponse stubbedCensusResponse = StubbedCensusResponse.Create();

        _handler
            .Handle(Arg.Any<GetCensusActionsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success(stubbedCensusResponse));

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            $"/census/{_censusId}",
            user,
            CancellationToken.None
        );

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        CensusActionsResponse result =
            await response.Content.ReadFromJsonAsync<CensusActionsResponse>(CancellationToken.None);

        result.ShouldNotBeNull();
        result.Title.ShouldBe(stubbedCensusResponse.Title);
        result.Caption.ShouldBe(stubbedCensusResponse.Caption);
        result.Overview.ShouldBe(stubbedCensusResponse.Overview);
        result.Status.Name.ShouldBe(stubbedCensusResponse.Status.Name);
        result.Status.Label.ShouldBe(stubbedCensusResponse.Status.Label);
        result.LastUpdated.Date.ShouldBe(stubbedCensusResponse.LastUpdated.Date);
        result.CallToAction.Label.ShouldBe(stubbedCensusResponse.CallToAction.Label);
        result.CallToAction.Url.ShouldBe(stubbedCensusResponse.CallToAction.Url);
        result.Steps.Count.ShouldBe(stubbedCensusResponse.Steps.Count);
        result.ImportantDates.Count.ShouldBe(stubbedCensusResponse.ImportantDates.Count);
    }

    [Fact]
    public async Task CensusActions_endpoint_should_pass_the_census_id_and_user_to_the_handler()
    {
        // Arrange
        User user = ValidUser();

        _handler
            .Handle(Arg.Any<GetCensusActionsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success(StubbedCensusResponse.Create()));

        // Act
        await _client.PostAsJsonAsync($"/census/{_censusId}", user, CancellationToken.None);

        // Assert
        await _handler
            .Received(1)
            .Handle(
                Arg.Is<GetCensusActionsQuery>(query =>
                    query!.Request.CensusId == _censusId
                    && query.Request.UserDetails.Id == "test-user-id"
                    && query.Request.UserDetails.OrgDetails.Count == 1
                ),
                Arg.Any<CancellationToken>()
            );
    }

    [Fact]
    public async Task CensusActions_endpoint_should_return_validation_problem_without_a_user()
    {
        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            $"/census/{_censusId}",
            new { },
            CancellationToken.None
        );

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        ValidationProblemDetails problem =
            await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(
                CancellationToken.None
            );

        problem.ShouldNotBeNull();
        problem.Errors.Keys.ShouldContain("User.Email");

        await _handler
            .DidNotReceive()
            .Handle(Arg.Any<GetCensusActionsQuery>(), Arg.Any<CancellationToken>());
    }

    private static User ValidUser()
    {
        return new User
        {
            Id = "test-user-id",
            Email = "test.user@email.com",
            Organisations =
            [
                new Organisation
                {
                    Id = "test-organisation-id",
                    Name = "test-organisation-name",
                    Category = new Category
                    {
                        Id = "test-category-id",
                        Name = "test-category-name",
                    },
                    Ukprn = "test-ukprn",
                },
            ],
        };
    }
}
