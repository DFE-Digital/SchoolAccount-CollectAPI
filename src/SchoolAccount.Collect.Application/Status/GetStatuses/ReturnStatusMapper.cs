namespace SchoolAccount.Collect.Application.Status.GetStatuses;

public static class ReturnStatusMapper
{
    private static readonly Dictionary<int, string> StatusDescriptions = new()
    {
        { 1, "No Data" },
        { 23, "Awaiting File Upload" },
        { 24, "File Upload In Progress" },
        { 31, "File Uploaded" },
        { 32, "File Upload Failed" },
        { 13, "Waiting For Validation" },
        { 12, "Validation In Progress" },
        { 11, "Uploaded Validation Failed" },
        { 2, "Loaded And Validated" },
        { 3, "Amended By Source" },
        { 14, "Awaiting Submission" },
        { 17, "Submission In Progress" },
        { 4, "Submitted" },
        { 5, "Rejected" },
        { 6, "Amended By Agent" },
        { 15, "Awaiting Approval" },
        { 18, "Approval In Progress" },
        { 7, "Approved" },
        { 8, "Rejected By Collector" },
        { 9, "Amended By Collector" },
        { 16, "Awaiting Authorisation" },
        { 19, "Authorisation In Progress" },
        { 10, "Authorised" },
        { 25, "Awaiting Matching" },
        { 26, "Matching In Progress" },
        { 27, "Awaiting Reconciliation" },
        { 28, "Reconciliation In Progress" },
        { 29, "Matching Failed" },
        { 30, "Reconciliation Failed" },
    };

    public static string GetStatusDescription(int statusCode)
    {
        return StatusDescriptions.TryGetValue(statusCode, out string? description)
            ? description
            : "Not Started";
    }

    public static string GetStatusName(int statusCode)
    {
        return GetStatusDescription(statusCode).Replace(" ", "");
    }
}
