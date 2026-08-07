using SchoolAccount.Collect.SharedKernel;

namespace SchoolAccount.Collect.Infrastructure.Time;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
