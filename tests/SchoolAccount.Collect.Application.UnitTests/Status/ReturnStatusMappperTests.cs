using SchoolAccount.Collect.Application.Status.GetStatuses;
using Shouldly;

namespace SchoolAccount.Collect.Application.UnitTests.Status;

public class ReturnStatusMappperTests
{
    [Theory]
    [InlineData(1, "No_Data")]
    [InlineData(23, "Awaiting_File_Upload")]
    [InlineData(24, "File_Upload_in_Progress")]
    [InlineData(31, "File_Uploaded")]
    [InlineData(32, "File_Upload_Failed")]
    [InlineData(13, "Waiting_for_validation")]
    [InlineData(12, "Validation_in_progress")]
    [InlineData(11, "Uploaded_Validation_Failed")]
    [InlineData(2, "Loaded_and_Validated")]
    [InlineData(3, "Amended_by_source")]
    [InlineData(14, "Awaiting_Submission")]
    [InlineData(17, "Submission_in_Progress")]
    [InlineData(4, "Submitted")]
    [InlineData(5, "Rejected")]
    [InlineData(6, "Amended_by_agent")]
    [InlineData(15, "Awaiting_Approval")]
    [InlineData(18, "Approval_in_Progress")]
    [InlineData(7, "Approved")]
    [InlineData(8, "Rejected_by_collector")]
    [InlineData(9, "Amended_by_collector")]
    [InlineData(16, "Awaiting_Authorisation")]
    [InlineData(19, "Authorisation_In_Progress")]
    [InlineData(10, "Authorised")]
    [InlineData(25, "Awaiting_Matching")]
    [InlineData(26, "Matching_in_Progress")]
    [InlineData(27, "Awaiting_Reconciliation")]
    [InlineData(28, "Reconciliation_in_Progress")]
    [InlineData(29, "Matching_Failed")]
    [InlineData(30, "Reconciliation_Failed")]
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
