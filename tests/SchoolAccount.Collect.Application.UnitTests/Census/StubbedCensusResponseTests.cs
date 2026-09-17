using SchoolAccount.Collect.Application.Census.GetCensusActions;
using Shouldly;

namespace SchoolAccount.Collect.Application.UnitTests.Census;

public class StubbedCensusResponseTests
{
    [Fact]
    public void Create_without_statusCode_should_return_not_started_status()
    {
        // Act
        CensusActionsResponse response = StubbedCensusResponse.Create();

        // Assert
        response.Status.Name.ShouldBe("notStarted");
        response.Status.Label.ShouldBe("Unavailable");
    }

    [Theory]
    [InlineData(7, "Approved", "Approved")]
    [InlineData(9, "Amended_by_collector", "Amended_by_collector")]
    [InlineData(10, "Authorised", "Authorised")]
    [InlineData(16, "Awaiting_Authorisation", "Awaiting_Authorisation")]
    public void Create_with_statusCode_should_match_status_correctly(
        int statusCode,
        string expectedName,
        string expectedLabel
    )
    {
        // Act
        CensusActionsResponse response = StubbedCensusResponse.Create(statusCode);

        // Assert
        response.Status.Name.ShouldBe(expectedName);
        response.Status.Label.ShouldBe(expectedLabel);
    }

    [Fact]
    public void Create_with_invalid_statusCode_should_return_not_started_status()
    {
        // Act & Assert
        CensusActionsResponse response = StubbedCensusResponse.Create(100);
        response.Status.Name.ShouldBe("Unavailable");
    }
}
