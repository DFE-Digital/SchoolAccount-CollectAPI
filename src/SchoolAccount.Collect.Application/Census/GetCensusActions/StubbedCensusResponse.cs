namespace SchoolAccount.Collect.Application.Census.GetCensusActions;

public static class StubbedCensusResponse
{
    public static CensusActionsResponse Create()
    {
        return new CensusActionsResponse
        {
            Title = "Autumn School Census",
            Caption = "Complete your census return",
            Overview =
                "The school census collects pupil and school data from state-funded schools three times a year. The data is exported from each school’s management information system (MIS) and submitted to the Department for Education through an online tool called COLLECT.",
            Status = new ActionStatus { Name = "notStarted", Label = "Not Started" },
            LastUpdated = new LastUpdated { Date = new DateOnly(2026, 8, 26) },
            CallToAction = new CallToAction
            {
                Label = "Go to Autumn Census 2026",
                Url = "/collect.education.gov.uk",
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
        };
    }
}
