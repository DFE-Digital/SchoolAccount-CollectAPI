using Microsoft.Extensions.Options;
using NSubstitute;
using SchoolAccount.Collect.Application.Census.GetCensusActions;
using SchoolAccount.Collect.Application.Configuration;
using SchoolAccount.Collect.Infrastructure.Census;

namespace SchoolAccount.Collect.Infrastructure.UnitTests;

public class CensusReturnStatusReaderTests
{
    private readonly ICensusReturnStatusReader _returnStatusReader =
        Substitute.For<ICensusReturnStatusReader>();

    [Fact]
    public async Task GetStatus_returns_correct_status_when_database_is_used()
    {
        // Arrange
        _returnStatusReader
            .GetReturnStatusCode(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(StatusCode.AmendedByCollector);
        var reader = CensusReturnStatusReader(CreateSettings(true));

        // Act
    }

    private static IOptionsSnapshot<CensusSettings> CreateSettings(bool useDatabase)
    {
        IOptionsSnapshot<CensusSettings> settings = Substitute.For<
            IOptionsSnapshot<CensusSettings>
        >();

        settings.Value.Returns(new CensusSettings { UseDatabase = useDatabase });

        return settings;
    }
}
