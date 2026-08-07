using SchoolAccount.Collect.SharedKernel;

namespace SchoolAccount.Collect.Application.Organisations.GetByLaestab;

public class StatusCalculator(IDateTimeProvider dateTimeProvider)
{
    private static readonly TimeSpan StartOfDay = new(8, 0, 0);
    private static readonly TimeSpan EndOfDay = new(15, 30, 0);
    private static readonly TimeZoneInfo BritishTimeZone = TimeZoneInfo.FindSystemTimeZoneById(
        "GMT Standard Time"
    );

    public OrgStatus GetOpenStatus()
    {
        DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(
            dateTimeProvider.UtcNow,
            BritishTimeZone
        );
        return IsWeekend(localTime) || !IsWithinSchoolHours(localTime)
            ? OrgStatus.Closed
            : OrgStatus.Open;
    }

    private static bool IsWeekend(DateTime localTime) =>
        localTime.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    private static bool IsWithinSchoolHours(DateTime localTime) =>
        localTime.TimeOfDay >= StartOfDay && localTime.TimeOfDay < EndOfDay;
}
