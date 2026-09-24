using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Collect.Application.Status.GetStatuses;

namespace SchoolAccount.Collect.Application.Census.GetCensusActions;

[SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded")]
public static class StubbedCensusResponse
{
    public static CensusActionsResponse Create(int? statusCode = null)
    {
        return new CensusActionsResponse
        {
            Title = "Autumn school census",
            Caption = "Complete your census return",
            Overview =
                "The school census collects pupil and school data from state-funded schools three times a year. The data is exported from each school’s management information system (MIS) and submitted to the Department for Education through an online tool called COLLECT.",
            Status = statusCode is null
                ? new ActionStatus { Name = "unavailable", Label = "Unavailable" }
                : new ActionStatus
                {
                    Name = ReturnStatusMapper.GetStatusName(statusCode.Value),
                    Label = ReturnStatusMapper.GetStatusDescription(statusCode.Value),
                },
            LastUpdated = new LastUpdated { Date = new DateOnly(2026, 8, 26) },
            SupportService = new SupportService
            {
                Title = "Get help with the Autumn school census",
                Description = "Contact the Autumn school census team (opens in new tab)",
                Url = new Uri(
                    "https://form.education.gov.uk/en/AchieveForms/?form_uri=sandbox-publish://AF-Process-2b61dfcd-9296-4f6a-8a26-4671265cae67/AF-Stage-f3f5200e-e605-4a1b-ae6b-3536bc77305c/definition.json&redirectlink=%2Fen&cancelRedirectLink=%2Fen"
                ),
            },
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
                        "Nothing has been uploaded to COLLECT yet. You need to upload your data.",
                },
                new UnderstandStatus
                {
                    Name = "Loaded",
                    Description = "Your data has been uploaded to COLLECT. You need to submit it.",
                },
                new UnderstandStatus
                {
                    Name = "Submitted",
                    Description =
                        "You have submitted your data. Your local authority or DfE will now check it.",
                },
                new UnderstandStatus
                {
                    Name = "Amended by Source",
                    Description =
                        "Your data has been changed since you submitted it. You need to submit it again.",
                },
                new UnderstandStatus
                {
                    Name = "Approved",
                    Description =
                        "Your data has been approved and is waiting for DfE to authorise it. DfE will contact you if you need to do anything else.",
                },
                new UnderstandStatus
                {
                    Name = "Authorised",
                    Description =
                        "DfE has accepted your data. You do not need to do anything else unless DfE contacts you.",
                },
            ],
        };
    }
}
