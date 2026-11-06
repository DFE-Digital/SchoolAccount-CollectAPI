using NSubstitute;
using SchoolAccount.Collect.Application.Census.GetCensusActions;
using SchoolAccount.Collect.Application.Shared;
using SchoolAccount.Collect.SharedKernel;
using Shouldly;

namespace SchoolAccount.Collect.Application.UnitTests.Census.GetCensusActions;

public class GetCensusActionsHandlerTests
{
    private readonly ICensusReturnStatusReader _returnStatusReader =
        Substitute.For<ICensusReturnStatusReader>();

    [Fact]
    public async Task A_census_response_is_returned_for_a_valid_user_and_organisation()
    {
        // Arrange
        GetCensusActionsQuery query = CreateQuery();

        _returnStatusReader
            .GetReturnStatusCode(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(null as int?);

        var handler = new GetCensusActionsHandler(_returnStatusReader);

        CensusActionsResponse censusResponse = StubbedCensusResponse.Create();

        // Act
        Result<CensusActionsResponse> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Title.ShouldBe(censusResponse.Title);
        result.Value.Caption.ShouldBe(censusResponse.Caption);
        result.Value.Overview.ShouldBe(censusResponse.Overview);
        result.Value.Status.Name.ShouldBe(censusResponse.Status.Name);
        result.Value.Status.Label.ShouldBe(censusResponse.Status.Label);
        result.Value.LastUpdated.Date.ShouldBe(censusResponse.LastUpdated.Date);
        result.Value.CallToAction.Label.ShouldBe(censusResponse.CallToAction.Label);
        result.Value.CallToAction.Url.ShouldBe(censusResponse.CallToAction.Url);
        result.Value.Steps.Count.ShouldBe(censusResponse.Steps.Count);
        result.Value.ImportantDates.Count.ShouldBe(censusResponse.ImportantDates.Count);
    }

    [Fact]
    public async Task The_database_is_used_when_enabled()
    {
        // Arrange
        GetCensusActionsQuery query = CreateQuery();

        _returnStatusReader
            .GetReturnStatusCode(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(9);

        var handler = new GetCensusActionsHandler(_returnStatusReader);

        // Act
        Result<CensusActionsResponse> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Status.Name.ShouldBe("Amended_by_collector");
        result.Value.Status.Label.ShouldBe("Amended_by_collector");

        await _returnStatusReader
            .Received(1)
            .GetReturnStatusCode(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task A_user_without_an_organisation_throws()
    {
        // Arrange
        var request = new GetCensusActionsRequestModel
        {
            CensusId = "test-id",
            UserDetails = new UserDetails
            {
                Id = "test-user-id",
                Email = "test.user@email.com",
                OrgDetails = [],
            },
        };

        var query = new GetCensusActionsQuery(request);

        var handler = new GetCensusActionsHandler(_returnStatusReader);

        // Act & Assert
        await Should.ThrowAsync<ArgumentException>(() =>
            handler.Handle(query, CancellationToken.None)
        );
    }

    private static GetCensusActionsQuery CreateQuery()
    {
        var requestModel = new GetCensusActionsRequestModel
        {
            CensusId = "test-census-id",
            UserDetails = new UserDetails
            {
                Id = "test-user-id",
                Email = "test.user@email.com",
                OrgDetails =
                [
                    new OrgDetails
                    {
                        Id = "test-organisation-id",
                        Name = "test-organisation-name",
                        CategoryId = "test-category-id",
                        Ukprn = "test-ukprn",
                    },
                ],
            },
        };

        return new GetCensusActionsQuery(requestModel);
    }
}
