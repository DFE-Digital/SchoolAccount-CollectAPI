using SchoolAccount.Collect.Api.Endpoints.Census.GetCensusActions;
using SchoolAccount.Collect.Api.Endpoints.Shared;
using SchoolAccount.Collect.Api.Endpoints.Status.GetStatuses;
using SchoolAccount.Collect.Application.Census.GetCensusActions;
using Shouldly;

namespace SchoolAccount.Collect.Api.UnitTests.Endpoints.Census;

public class GetCensusActionsRequestTests
{
    [Fact]
    public void Query_is_created_correctly_with_valid_request()
    {
        // Arrange
        var request = new GetCensusActionsRequest
        {
            CensusId = "test-census-id",
            User = new User
            {
                Id = "test-user-id",
                Email = "test.user@email.com",
                Organisations = new List<Organisation>
                {
                    new()
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
            },
        };

        // Act
        GetCensusActionsQuery query = request.ToQuery();

        // Assert
        query.Request.CensusId.ShouldBe(request.CensusId);
        query.Request.UserDetails.Id.ShouldBe(request.User.Id);
        query.Request.UserDetails.Email.ShouldBe(request.User.Email);
        query.Request.UserDetails.OrgDetails[0].Id.ShouldBe(request.User.Organisations[0].Id);
        query.Request.UserDetails.OrgDetails[0].Name.ShouldBe(request.User.Organisations[0].Name);
        query
            .Request.UserDetails.OrgDetails[0]
            .CategoryId.ShouldBe(request.User.Organisations[0].Category.Id);
        query.Request.UserDetails.OrgDetails[0].Ukprn.ShouldBe(request.User.Organisations[0].Ukprn);
        query.Request.UserDetails.OrgDetails[0].LocalAuthorityCode.ShouldBe("123");
        query.Request.UserDetails.OrgDetails[0].EstablishmentNumber.ShouldBe("4567");
    }
}
