using SchoolAccount.Application.Organisations.GetByLaestab;
using Shouldly;

namespace SchoolAccount.Application.UnitTests.Organisations;

public class LaestabValueTests
{
    /*
      The LaestabValue is only ever run against sanitised strings of 7 digits,
      no further test are necessary
    */
    [Fact]
    public void Laestab_is_separated_into_localAuthorityCode_and_establishmentNumber_by_LaestabValue()
    {
        // Arrange
        const string localAuthorityCode = "321";
        const string establishmentNo = "4567";
        const string laestab = localAuthorityCode + establishmentNo;

        // Act
        var laestabValue = new LaestabValue(laestab);

        // Assert
        laestabValue.LocalAuthorityCode.ShouldBe(localAuthorityCode);
        laestabValue.EstablishmentNo.ShouldBe(establishmentNo);
    }

    [Fact]
    public void Throw_an_exception_when_the_laestab_value_is_not_seven_characters_long()
    {
        // Arrange
        const string laestabValue = "123456";

        // Act
        ArgumentException exception = Should.Throw<ArgumentException>(() =>
            new LaestabValue(laestabValue)
        );

        // Assert
        exception.Message.ShouldBe("Laestab must be 7 characters long");
    }
}
