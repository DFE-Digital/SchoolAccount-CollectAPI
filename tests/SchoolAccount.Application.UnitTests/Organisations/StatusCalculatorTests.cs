using NSubstitute;
using SchoolAccount.Application.Organisations.GetByLaestab;
using SchoolAccount.SharedKernel;
using Shouldly;

namespace SchoolAccount.Application.UnitTests.Organisations;

public class StatusCalculatorTests
{
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();

    private OrgStatus GetStatusAt(DateTime utc)
    {
        _dateTimeProvider.UtcNow.Returns(utc);
        return new StatusCalculator(_dateTimeProvider).GetOpenStatus();
    }

    [Fact]
    public void Organisation_is_open_during_school_hours_on_a_weekday()
    {
        // Arrange
        var nineAm = new DateTime(2026, 2, 11, 9, 0, 0, DateTimeKind.Utc);

        // Act & Assert
        GetStatusAt(nineAm).ShouldBe(OrgStatus.Open);
    }

    [Fact]
    public void Organisation_is_open_at_exact_opening_time_on_a_weekday()
    {
        // Arrange
        var exactOpeningTime = new DateTime(2026, 2, 11, 8, 0, 0, DateTimeKind.Utc);

        // Act & Assert
        GetStatusAt(exactOpeningTime).ShouldBe(OrgStatus.Open);
    }

    [Fact]
    public void Organisation_is_closed_at_exact_closing_time_on_a_weekday()
    {
        // Arrange
        var exactClosingTime = new DateTime(2026, 2, 11, 15, 30, 0, DateTimeKind.Utc);

        // Act & Assert
        GetStatusAt(exactClosingTime).ShouldBe(OrgStatus.Closed);
    }

    [Fact]
    public void Organisation_is_closed_outside_of_school_hours_on_a_weekday()
    {
        // Arrange
        var tenPm = new DateTime(2026, 2, 11, 22, 00, 0, DateTimeKind.Utc);

        // Act & Assert
        GetStatusAt(tenPm).ShouldBe(OrgStatus.Closed);
    }

    [Fact]
    public void Organisation_takes_into_account_daylight_savings_for_open_on_a_weekday()
    {
        // Arrange
        // 8:30 AM BST = 7:30 AM UTC
        var eightThirtyAmBST = new DateTime(2026, 7, 15, 7, 30, 0, DateTimeKind.Utc);

        // Act & Assert
        GetStatusAt(eightThirtyAmBST).ShouldBe(OrgStatus.Open);
    }

    [Fact]
    public void Organisation_takes_into_account_daylight_savings_for_closed_on_a_weekday()
    {
        // Arrange
        // 7:59 AM BST = 6:59 AM UTC
        var beforeOpeningBST = new DateTime(2026, 7, 15, 6, 59, 0, DateTimeKind.Utc);

        // Act & Assert
        GetStatusAt(beforeOpeningBST).ShouldBe(OrgStatus.Closed);
    }

    [Fact]
    public void Organisation_is_closed_on_saturday_during_school_hours()
    {
        // Arrange
        var saturdayNineAm = new DateTime(2026, 2, 14, 9, 0, 0, DateTimeKind.Utc);

        // Act & Assert
        GetStatusAt(saturdayNineAm).ShouldBe(OrgStatus.Closed);
    }

    [Fact]
    public void Organisation_is_closed_on_sunday_during_school_hours()
    {
        // Arrange
        var sundayNineAm = new DateTime(2026, 2, 15, 9, 0, 0, DateTimeKind.Utc);

        // Act & Assert
        GetStatusAt(sundayNineAm).ShouldBe(OrgStatus.Closed);
    }
}
