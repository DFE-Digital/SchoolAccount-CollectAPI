using SchoolAccount.Collect.Application.Status.GetStatuses;
using Shouldly;

namespace SchoolAccount.Collect.Application.UnitTests.Status;

public class ReturnStatusMappperTests
{
    [Theory]
    [InlineData(1, "No Data")]
    [InlineData(23, "Awaiting File Upload")]
    [InlineData(24, "File Upload in Progress")]
    [InlineData(31, "File Uploaded")]
    [InlineData(32, "File Upload Failed")]
    [InlineData(13, "Waiting for validation")]
    [InlineData(12, "Validation in progress")]
    [InlineData(11, "Uploaded Validation Failed")]
    [InlineData(2, "Loaded and Validated")]
    [InlineData(3, "Amended by source")]
    [InlineData(14, "Awaiting Submission")]
    [InlineData(17, "Submission in Progress")]
    [InlineData(4, "Submitted")]
    [InlineData(5, "Rejected")]
    [InlineData(6, "Amended by agent")]
    [InlineData(15, "Awaiting Approval")]
    [InlineData(18, "Approval in Progress")]
    [InlineData(7, "Approved")]
    [InlineData(8, "Rejected by collector")]
    [InlineData(9, "Amended by collector")]
    [InlineData(16, "Awaiting Authorisation")]
    [InlineData(19, "Authorisation In Progress")]
    [InlineData(10, "Authorised")]
    [InlineData(25, "Awaiting Matching")]
    [InlineData(26, "Matching in Progress")]
    [InlineData(27, "Awaiting Reconciliation")]
    [InlineData(28, "Reconciliation in Progress")]
    [InlineData(29, "Matching Failed")]
    [InlineData(30, "Reconciliation Failed")]
    public void should_return_the_correct_status_description_for_a_given_status_code(
        int returnCode,
        string expectedDescription
    )
    {
        // Arrange and Act
        string statusDescription = ReturnStatusMapper.GetStatusDescription(returnCode);

        // Assert
        statusDescription.ShouldBe(expectedDescription);
    }
}
