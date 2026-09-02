using SchoolAccount.Collect.Application.Census.GetCensusActions;
using SchoolAccount.Collect.Application.Shared;
using SchoolAccount.Collect.Application.Status.GetStatuses;
using SchoolAccount.Collect.SharedKernel;
using Shouldly;

namespace SchoolAccount.Collect.Application.UnitTests.Census;

public class GetCensusActionsHandlerTests
{
    [Fact]
    public async Task Handler_takes_a_getcensusactionsrequestmodel_and_returns_a_censusactionsresponse()
    {
        // Arrange
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

        var query = new GetCensusActionsQuery(requestModel);

        var handler = new GetCensusActionsHandler();

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
}
