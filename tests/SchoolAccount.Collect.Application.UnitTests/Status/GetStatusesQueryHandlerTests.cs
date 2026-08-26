using SchoolAccount.Collect.Application.Status.GetStatuses;
using SchoolAccount.Collect.SharedKernel;
using Shouldly;

namespace SchoolAccount.Collect.Application.UnitTests.Status;

public class GetStatusesQueryHandlerTests
{
    [Fact]
    public async Task Handler_takes_a_getstatusrequestmodel_and_returns_a_statusresponse()
    {
        // Arrange
        var requestModel = new GetStatusesRequestModel
        {
            OrgDetails = new List<OrgDetails>
            {
                new()
                {
                    Id = "test-id",
                    Name = "test-name",
                    CategoryId = "test-category-id",
                    Ukprn = "test-ukprn",
                },
            },
        };

        var query = new GetStatusesQuery(requestModel);

        var handler = new GetStatusesQueryHandler();

        // Act
        Result<StatusResponse> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Details[0].Id.ShouldBe("test-id");
        result.Value.Details[0].Name.ShouldBe("test-name");
        result.Value.Details[0].CategoryId.ShouldBe("test-category-id");
        result.Value.Details[0].Ukprn.ShouldBe("test-ukprn");
        result.Value.Details[0].Interesting.ShouldBeFalse();
    }
}
