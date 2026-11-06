using SchoolAccount.Collect.Application.Census.GetCensusActions;
using Shouldly;

namespace SchoolAccount.Collect.Application.UnitTests.Census.GetCensusActions;

public class StubbedCensusResponseTests
{
    [Fact]
    public void An_unavailable_status_is_returned_for_no_status_code()
    {
        // Act
        CensusActionsResponse response = StubbedCensusResponse.Create();

        // Assert
        response.Status.Name.ShouldBe("unavailable");
        response.Status.Label.ShouldBe("Unavailable");
    }

    [Theory]
    [InlineData(7, "Approved", "Approved")]
    [InlineData(9, "Amended_by_collector", "Amended_by_collector")]
    [InlineData(10, "Authorised", "Authorised")]
    [InlineData(16, "Awaiting_Authorisation", "Awaiting_Authorisation")]
    public void The_status_is_correctly_mapped(
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
    public void An_unavailable_status_is_returned_for_an_invalid_status_code()
    {
        // Act & Assert
        CensusActionsResponse response = StubbedCensusResponse.Create(100);
        response.Status.Name.ShouldBe("Unavailable");
    }
}
