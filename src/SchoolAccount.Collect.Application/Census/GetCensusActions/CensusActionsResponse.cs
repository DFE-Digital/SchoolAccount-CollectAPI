namespace SchoolAccount.Collect.Application.Census.GetCensusActions;

public sealed class CensusActionsResponse
{
    public required string Title { get; init; }
    public required string Caption { get; init; }
    public required string Overview { get; init; }
    public required ActionStatus Status { get; init; }
    public required LastUpdated LastUpdated { get; init; }
    public required CallToAction CallToAction { get; init; }
    public IReadOnlyList<ActionStep> Steps { get; init; } = [];
    public IReadOnlyList<ImportantDate> ImportantDates { get; init; } = [];
}

public sealed class ActionStatus
{
    public required string Name { get; init; }
    public required string Label { get; init; }
}

public sealed class LastUpdated
{
    public DateTimeOffset Date { get; init; }
}

public sealed class CallToAction
{
    public required string Label { get; init; }
    public required string Url { get; init; }
}

public sealed class ActionStep
{
    public int Order { get; init; }
    public required string Title { get; init; }
    public required string Body { get; init; }
    public DateTag? DateTag { get; init; }
}

public sealed class DateTag
{
    public DateOnly Date { get; init; }
    public required string Prefix { get; init; }
}

public sealed class ImportantDate
{
    public DateOnly Date { get; init; }
    public required string Label { get; init; }
}
