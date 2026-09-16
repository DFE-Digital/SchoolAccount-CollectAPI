namespace SchoolAccount.Collect.Application.Configuration;

public class CensusSettings
{
    public const string SectionName = "Census";

    public bool UseDatabase { get; init; }
    public string? ConnectionString { get; init; }
    public string CurrentOpenCensus { get; init; }
    public string CurrentOpenCensusDisplayName { get; init; }
}
