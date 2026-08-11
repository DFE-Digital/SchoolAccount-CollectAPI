using NSubstitute;
using SchoolAccount.Collect.Application.Organisations.GetByLaestab;
using SchoolAccount.Collect.SharedKernel;
using Shouldly;

namespace SchoolAccount.Collect.Application.UnitTests.Organisations;

public class GetOrganisationByLaestabQueryHandlerTests
{
    [Fact]
    public async Task Handler_takes_a_laestab_and_returns_school_open_status_localAuthorityCode_and_establishmentNumber()
    {
        // arrange
        string localAuthorityCode = "321";
        string establishmentNo = "4567";
        string laestab = localAuthorityCode + establishmentNo;
        var onePm = new DateTime(2026, 7, 17, 13, 0, 0, DateTimeKind.Utc);

        IDateTimeProvider dateTimeProvider = Substitute.For<IDateTimeProvider>();
        dateTimeProvider.UtcNow.Returns(onePm);
        var handler = new GetOrganisationByLaestabQueryHandler(dateTimeProvider);

        // act
        Result<OrganisationResponse> result = await handler.Handle(
            new GetOrganisationByLaestabQuery(laestab),
            CancellationToken.None
        );

        // assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Status.ShouldBe(OrgStatus.Open);
        result.Value.LocalAuthorityCode.ShouldBe(localAuthorityCode);
        result.Value.EstablishmentNumber.ShouldBe(establishmentNo);
    }
}
