using SchoolAccount.Collect.Application.Status.GetStatuses;
using Shouldly;

namespace SchoolAccount.Collect.Application.UnitTests.Status;

public class ReturnStatusMappperTests
{
    [Theory]
    [InlineData(1, "No Data")]
    [InlineData(23, "Awaiting File Upload")]
    [InlineData(24, "File Upload In Progress")]
    [InlineData(31, "File Uploaded")]
    [InlineData(32, "File Upload Failed")]
    [InlineData(13, "Waiting For Validation")]
    [InlineData(12, "Validation In Progress")]
    [InlineData(11, "Uploaded Validation Failed")]
    [InlineData(2, "Loaded And Validated")]
    [InlineData(3, "Amended By Source")]
    [InlineData(14, "Awaiting Submission")]
    [InlineData(17, "Submission In Progress")]
    [InlineData(4, "Submitted")]
    [InlineData(5, "Rejected")]
    [InlineData(6, "Amended By Agent")]
    [InlineData(15, "Awaiting Approval")]
    [InlineData(18, "Approval In Progress")]
    [InlineData(7, "Approved")]
    [InlineData(8, "Rejected By Collector")]
    [InlineData(9, "Amended By Collector")]
    [InlineData(16, "Awaiting Authorisation")]
    [InlineData(19, "Authorisation In Progress")]
    [InlineData(10, "Authorised")]
    [InlineData(25, "Awaiting Matching")]
    [InlineData(26, "Matching In Progress")]
    [InlineData(27, "Awaiting Reconciliation")]
    [InlineData(28, "Reconciliation In Progress")]
    [InlineData(29, "Matching Failed")]
    [InlineData(30, "Reconciliation Failed")]
    public void Returns_the_correct_status_description_for_a_given_status_code(
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
