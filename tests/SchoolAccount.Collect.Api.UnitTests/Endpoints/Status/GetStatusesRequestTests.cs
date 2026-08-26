using SchoolAccount.Collect.Api.Endpoints.Status.GetStatuses;
using SchoolAccount.Collect.Application.Status.GetStatuses;
using Shouldly;

namespace SchoolAccount.Collect.Api.UnitTests.Endpoints.Status;

public class GetStatusesRequestTests
{
    [Fact]
    public void Query_is_created_correctly_with_valid_trust_school_request()
    {
        // Arrange
        var request = new GetStatusesRequest
        {
            Id = "test-id",
            Email = "test.user@email.com",
            Organisations = new List<Organisation>
            {
                new Organisation
                {
                    Id = "test-organisation-id",
                    Name = "test-organisation-name",
                    Category = new Category
                    {
                        Id = "test-category-id",
                        Name = "test-category-name",
                    },
                    Ukprn = "test-ukprn",
                },
            },
        };

        // Act
        GetStatusesQuery query = request.ToQuery();

        // Assert
        query.Request.OrgDetails[0].Id.ShouldBe(request.Organisations[0].Id);
        query.Request.OrgDetails[0].Name.ShouldBe(request.Organisations[0].Name);
        query.Request.OrgDetails[0].CategoryId.ShouldBe(request.Organisations[0].Category.Id);
        query.Request.OrgDetails[0].Ukprn.ShouldBe(request.Organisations[0].Ukprn);
        query.Request.OrgDetails[0].LocalAuthorityCode.ShouldBeNull();
        query.Request.OrgDetails[0].EstablishmentNumber.ShouldBeNull();
    }

    [Fact]
    public void Query_is_created_correctly_with_valid_academy_request()
    {
        // Arrange
        var request = new GetStatusesRequest
        {
            Id = "test-id",
            Email = "test.user@email.com",
            Organisations = new List<Organisation>
            {
                new Organisation
                {
                    Id = "test-organisation-id",
                    Name = "test-organisation-name",
                    Category = new Category
                    {
                        Id = "test-category-id",
                        Name = "test-category-name",
                    },
                    Ukprn = "test-ukprn",
                    LocalAuthority = new LocalAuthority
                    {
                        Id = "test-la-id",
                        Name = "test-la-name",
                        Code = "123",
                    },
                    EstablishmentNumber = "4567",
                },
            },
        };

        // Act
        GetStatusesQuery query = request.ToQuery();

        // Assert
        query.Request.OrgDetails[0].Id.ShouldBe(request.Organisations[0].Id);
        query.Request.OrgDetails[0].Name.ShouldBe(request.Organisations[0].Name);
        query.Request.OrgDetails[0].CategoryId.ShouldBe(request.Organisations[0].Category.Id);
        query.Request.OrgDetails[0].Ukprn.ShouldBe(request.Organisations[0].Ukprn);
        query
            .Request.OrgDetails[0]
            .LocalAuthorityCode.ShouldBe(request.Organisations[0].LocalAuthority?.Code);
        query
            .Request.OrgDetails[0]
            .EstablishmentNumber.ShouldBe(request.Organisations[0].EstablishmentNumber);
    }
}
