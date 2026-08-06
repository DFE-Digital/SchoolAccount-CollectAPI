using SchoolAccount.SharedKernel;

namespace SchoolAccount.Infrastructure.Time;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
