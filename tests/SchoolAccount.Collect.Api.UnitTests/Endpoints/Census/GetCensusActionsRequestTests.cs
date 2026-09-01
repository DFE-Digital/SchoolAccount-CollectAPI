using SchoolAccount.Collect.Api.Endpoints.Census.GetCensusActions;
using SchoolAccount.Collect.Application.Census.GetCensusActions;
using Shouldly;

namespace SchoolAccount.Collect.Api.UnitTests.Endpoints.Census;

public class GetCensusActionsRequestTests
{
    [Fact]
    public void Query_is_created_correctly_with_valid_request()
    {
        // Arrange
        var request = new GetCensusActionsRequest { Id = "test-id" };

        // Act
        GetCensusActionsQuery query = request.ToQuery();

        // Assert
        query.Request.Id.ShouldBe(request.Id);
    }
}
