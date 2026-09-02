using SchoolAccount.Collect.Api.Endpoints.Census.GetCensusActions;
using SchoolAccount.Collect.Api.Endpoints.Shared;
using SchoolAccount.Collect.Api.Endpoints.Status.GetStatuses;
using SchoolAccount.Collect.Application.Census.GetCensusActions;
using SchoolAccount.Collect.Application.Shared;
using SchoolAccount.Collect.Application.Status.GetStatuses;
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
                Organisations =
                [
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
                ],
            },
        };

        // Act
        GetCensusActionsQuery query = request.ToQuery();

        // Assert
        UserDetails userDetails = query.Request.UserDetails;
        OrgDetails orgDetails = userDetails.OrgDetails[0];
        Organisation firstOrganisation = request.User.Organisations[0];

        query.Request.CensusId.ShouldBe(request.CensusId);
        userDetails.Id.ShouldBe(request.User.Id);
        userDetails.Email.ShouldBe(request.User.Email);
        orgDetails.Id.ShouldBe(firstOrganisation.Id);
        orgDetails.Name.ShouldBe(firstOrganisation.Name);
        orgDetails.CategoryId.ShouldBe(firstOrganisation.Category.Id);
        orgDetails.Ukprn.ShouldBe(firstOrganisation.Ukprn);
        orgDetails.LocalAuthorityCode.ShouldBe("123");
        orgDetails.EstablishmentNumber.ShouldBe("4567");
    }
}
