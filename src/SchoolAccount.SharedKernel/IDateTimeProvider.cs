namespace SchoolAccount.SharedKernel;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
