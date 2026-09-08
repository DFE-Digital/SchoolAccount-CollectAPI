using Microsoft.Extensions.Options;
using NSubstitute;
using SchoolAccount.Collect.Application.Census.GetCensusActions;
using SchoolAccount.Collect.Application.Configuration;
using SchoolAccount.Collect.Infrastructure.Census;
using Shouldly;

namespace SchoolAccount.Collect.Infrastructure.UnitTests.Census;

public class CensusReturnStatusReaderTests
{
    [Fact]
    public async Task GetReturnStatusCode_returns_null_when_database_is_disabled()
    {
        // Arrange
        var reader = new CensusReturnStatusReader(CreateSettings(false));

        // Act
        StatusCode? result = await reader.GetReturnStatusCode("1234567", CancellationToken.None);

        // Assert
        result.ShouldBeNull();
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
