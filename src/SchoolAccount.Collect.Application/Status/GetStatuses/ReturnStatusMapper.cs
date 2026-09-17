namespace SchoolAccount.Collect.Application.Status.GetStatuses;

public static class ReturnStatusMapper
{
    private static readonly Dictionary<int, string> StatusDescriptions = new()
    {
        { 1, "No Data" },
        { 23, "Awaiting File Upload" },
        { 24, "File Upload in Progress" },
        { 31, "File Uploaded" },
        { 32, "File Upload Failed" },
        { 13, "Waiting for validation" },
        { 12, "Validation in progress" },
        { 11, "Uploaded Validation Failed" },
        { 2, "Loaded and Validated" },
        { 3, "Amended by source" },
        { 14, "Awaiting Submission" },
        { 17, "Submission in Progress" },
        { 4, "Submitted" },
        { 5, "Rejected" },
        { 6, "Amended by agent" },
        { 15, "Awaiting Approval" },
        { 18, "Approval in Progress" },
        { 7, "Approved" },
        { 8, "Rejected by collector" },
        { 9, "Amended by collector" },
        { 16, "Awaiting Authorisation" },
        { 19, "Authorisation In Progress" },
        { 10, "Authorised" },
        { 25, "Awaiting Matching" },
        { 26, "Matching in Progress" },
        { 27, "Awaiting Reconciliation" },
        { 28, "Reconciliation in Progress" },
        { 29, "Matching Failed" },
        { 30, "Reconciliation Failed" },
    };

    public static string GetStatusDescription(int statusCode)
    {
        return StatusDescriptions.TryGetValue(statusCode, out string? description)
            ? description
            : "Not started";
    }
}
