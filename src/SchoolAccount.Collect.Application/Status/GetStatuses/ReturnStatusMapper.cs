namespace SchoolAccount.Collect.Application.Status.GetStatuses;

public static class ReturnStatusMapper
{
    private static readonly Dictionary<int, string> StatusDescriptions = new()
    {
        { 1, "No_Data" },
        { 23, "Awaiting_File_Upload" },
        { 24, "File_Upload_in_Progress" },
        { 31, "File_Uploaded" },
        { 32, "File_Upload_Failed" },
        { 13, "Waiting_for_validation" },
        { 12, "Validation_in_progress" },
        { 11, "Uploaded_Validation_Failed" },
        { 2, "Loaded_and_Validated" },
        { 3, "Amended_by_source" },
        { 14, "Awaiting_Submission" },
        { 17, "Submission_in_Progress" },
        { 4, "Submitted" },
        { 5, "Rejected" },
        { 6, "Amended_by_agent" },
        { 15, "Awaiting_Approval" },
        { 18, "Approval_in_Progress" },
        { 7, "Approved" },
        { 8, "Rejected_by_collector" },
        { 9, "Amended_by_collector" },
        { 16, "Awaiting_Authorisation" },
        { 19, "Authorisation_In_Progress" },
        { 10, "Authorised" },
        { 25, "Awaiting_Matching" },
        { 26, "Matching_in_Progress" },
        { 27, "Awaiting_Reconciliation" },
        { 28, "Reconciliation_in_Progress" },
        { 29, "Matching_Failed" },
        { 30, "Reconciliation_Failed" },
    };

    public static string GetStatusDescription(int statusCode)
    {
        return StatusDescriptions.TryGetValue(statusCode, out string? description)
            ? description
            : "Not started";
    }
}
