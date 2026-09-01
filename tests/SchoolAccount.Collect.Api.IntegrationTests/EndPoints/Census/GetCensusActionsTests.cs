using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using NSubstitute;
using SchoolAccount.Collect.Api.Endpoints.Census.GetCensusActions;
using SchoolAccount.Collect.Application.Abstractions.Messaging;
using SchoolAccount.Collect.Application.Census.GetCensusActions;
using SchoolAccount.Collect.SharedKernel;
using Shouldly;

namespace SchoolAccount.Collect.Api.IntegrationTests.EndPoints.Census;

public class GetCensusActionsTests : IClassFixture<WebApplicationFactory<Program>>
{
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
        var request = new GetCensusActionsRequest { Id = "test-id" };

        CensusActionsResponse stubbedCensusResponse = StubbedCensusResponse.Create();

        _handler
            .Handle(Arg.Any<GetCensusActionsQuery>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success(stubbedCensusResponse));

        // Act
        HttpResponseMessage response = await _client.GetAsync(
            $"/census/{request.Id}",
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
    public async Task CensusActions_endpoint_should_return_not_found_for_missing_id()
    {
        // Arrange
        var request = new GetCensusActionsRequest();

        // Act
        HttpResponseMessage response = await _client.GetAsync(
            $"/census/{request.Id}",
            CancellationToken.None
        );

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
