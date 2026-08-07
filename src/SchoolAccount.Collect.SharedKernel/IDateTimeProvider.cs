namespace SchoolAccount.Collect.SharedKernel;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
