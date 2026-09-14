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
        response.Status.Label.ShouldBe("Not Started");
    }

    [Theory]
    [InlineData(StatusCode.Approved, "approved", "Approved")]
    [InlineData(StatusCode.AmendedByCollector, "amendedByCollector", "Amended By Collector")]
    [InlineData(StatusCode.Authorised, "authorised", "Authorised")]
    [InlineData(
        StatusCode.AwaitingAuthorisation,
        "awaitingAuthorisation",
        "Awaiting Authorisation"
    )]
    public void Create_with_statusCode_should_match_status_correctly(
        StatusCode statusCode,
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
    public void Create_with_invalid_statusCode_should_throw_exception()
    {
        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() =>
            StubbedCensusResponse.Create((StatusCode)100)
        );
    }
}
