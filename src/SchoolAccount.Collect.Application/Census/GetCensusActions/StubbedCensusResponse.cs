using System.Diagnostics.CodeAnalysis;

namespace SchoolAccount.Collect.Application.Census.GetCensusActions;

[SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded")]
public static class StubbedCensusResponse
{
    public static CensusActionsResponse Create(StatusCode? statusCode = null)
    {
        return new CensusActionsResponse
        {
            Title = "Autumn School Census",
            Caption = "Complete your census return",
            Overview =
                "The school census collects pupil and school data from state-funded schools three times a year. The data is exported from each school’s management information system (MIS) and submitted to the Department for Education through an online tool called COLLECT.",
            Status = statusCode is null
                ? new ActionStatus { Name = "notStarted", Label = "Not Started" }
                : ToActionStatus(statusCode.Value),
            LastUpdated = new LastUpdated { Date = new DateOnly(2026, 8, 26) },
            CallToAction = new CallToAction
            {
                Label = "Go to COLLECT",
                Url = new Uri("https://collectdata.education.gov.uk/CollectPortalLive/"),
            },
            Steps =
            [
                new ActionStep
                {
                    Order = 1,
                    Title = "Check the census dates",
                    Body = "",
                },
                new ActionStep
                {
                    Order = 2,
                    Title = "Prepare the data in your MIS",
                    Body = "",
                },
                new ActionStep
                {
                    Order = 3,
                    Title = "Test your data before you submit it",
                    Body = "",
                    DateTag = new DateTag { Date = new DateOnly(2026, 9, 3), Prefix = "Available" },
                },
                new ActionStep
                {
                    Order = 4,
                    Title = "Generate your return and get headteacher sign-off",
                    Body = "",
                },
                new ActionStep
                {
                    Order = 5,
                    Title = "Submit your return",
                    Body = "",
                    DateTag = new DateTag
                    {
                        Date = new DateOnly(2026, 10, 1),
                        Prefix = "Available",
                    },
                },
                new ActionStep
                {
                    Order = 6,
                    Title = "Wait for DfE to check and authorise your return",
                    Body = "",
                },
            ],
            ImportantDates =
            [
                new ImportantDate { Date = new DateOnly(2026, 10, 1), Label = "Census day" },
                new ImportantDate { Date = new DateOnly(2026, 10, 28), Label = "Return due" },
            ],
            UnderstandStatuses =
            [
                new UnderstandStatus
                {
                    Name = "No Data",
                    Description =
                        "Nothing has been uploaded into COLLECT. The school needs to upload their data.",
                },
                new UnderstandStatus
                {
                    Name = "Submitted",
                    Description =
                        "The data has been uploaded by the source and submitted. DfE or your local authority will approve your data.",
                },
                new UnderstandStatus
                {
                    Name = "Approved",
                    Description =
                        "The data is being reviewed. DfE may need additional information before the return can be authorised. DfE will review your data. The school will be notified if additional information is needed.",
                },
                new UnderstandStatus
                {
                    Name = "Authorised",
                    Description =
                        "DfE have accepted the data. There are no more actions for the school.",
                },
            ],
        };
    }

    private static ActionStatus ToActionStatus(StatusCode statusCode)
    {
        return statusCode switch
        {
            StatusCode.Approved => new ActionStatus { Name = "approved", Label = "Approved" },
            StatusCode.AmendedByCollector => new ActionStatus
            {
                Name = "amendedByCollector",
                Label = "Amended By Collector",
            },
            StatusCode.Authorised => new ActionStatus { Name = "authorised", Label = "Authorised" },
            StatusCode.AwaitingAuthorisation => new ActionStatus
            {
                Name = "awaitingAuthorisation",
                Label = "Awaiting Authorisation",
            },
            _ => throw new ArgumentOutOfRangeException(nameof(statusCode)),
        };
    }
}
