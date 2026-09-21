using SchoolAccount.Collect.Application.Census.GetCensusActions;
using SchoolAccount.Collect.Application.Status.GetStatuses;
using Shouldly;
using Action = SchoolAccount.Collect.Application.Status.GetStatuses.Action;

namespace SchoolAccount.Collect.Application.UnitTests.Status;

public class StatusResponseBuilderTests
{
    [Fact]
    public void Populates_actions_from_census_return_for_a_school()
    {
        // Arrange
        var censusReturn = new CensusReturn
        {
            CollectionId = "test-collection-id",
            CollectionName = "Test Collection",
            StatusRows = [new() { LAEStab = "1234567", ReturnStatusCode = 7 }],
        };

        var responseBuilder = new StatusResponseBuilder(censusReturn);

        // Act
        OrgDetails testSchoolDetails = new()
        {
            Id = "2E774B32-E4DB-445B-B915-736C777FF5A4",
            Name = "Test Organisation",
            CategoryId = "001",
            Ukprn = "111223333",
            LocalAuthorityCode = "123",
            EstablishmentNumber = "4567",
        };

        StatusResponse response = responseBuilder.BuildResponse([testSchoolDetails]);

        // Assert
        response.Details.Count.ShouldBe(1);
        OrganisationResponse responseDetail = response.Details[0];
        responseDetail.Laestab.ShouldBe("1234567");
        responseDetail.Name.ShouldBe(testSchoolDetails.Name);
        responseDetail.CategoryId.ShouldBe(testSchoolDetails.CategoryId);
        responseDetail.Ukprn.ShouldBe(testSchoolDetails.Ukprn);
        responseDetail.Interesting.ShouldBeTrue();
        responseDetail.Actions.Count.ShouldBe(1);
        Action action = responseDetail.Actions[0];
        action.Name.ShouldBe(censusReturn.CollectionName);
        action.Id.ShouldBe(censusReturn.CollectionId);
        action.Status.Name.ShouldBe("Approved");
    }

    [Fact]
    public void Populates_action_as_unavailable_for_a_school_with_no_census_details()
    {
        // Arrange
        var censusReturn = new CensusReturn
        {
            CollectionId = "test-collection-id",
            CollectionName = "Test Collection",
            StatusRows = [],
        };

        var responseBuilder = new StatusResponseBuilder(censusReturn);

        // Act
        OrgDetails testSchoolDetails = new()
        {
            Id = "2E774B32-E4DB-445B-B915-736C777FF5A4",
            Name = "Test Organisation",
            CategoryId = "001",
            Ukprn = "111223333",
            LocalAuthorityCode = "123",
            EstablishmentNumber = "4567",
        };

        StatusResponse response = responseBuilder.BuildResponse([testSchoolDetails]);

        // Assert
        response.Details.Count.ShouldBe(1);
        OrganisationResponse responseDetail = response.Details[0];
        responseDetail.Laestab.ShouldBe("1234567");
        responseDetail.Name.ShouldBe(testSchoolDetails.Name);
        responseDetail.CategoryId.ShouldBe(testSchoolDetails.CategoryId);
        responseDetail.Ukprn.ShouldBe(testSchoolDetails.Ukprn);
        responseDetail.Interesting.ShouldBeTrue();
        responseDetail.Actions.Count.ShouldBe(1);
        Action action = responseDetail.Actions[0];
        action.Name.ShouldBe(censusReturn.CollectionName);
        action.Id.ShouldBe(censusReturn.CollectionId);
        action.Status.Name.ShouldBe("Unavailable");
    }

    [Fact]
    public void Actions_is_empty_and_interesting_is_false_for_trust_org_with_no_laestab()
    {
        // Arrange
        var censusReturn = new CensusReturn
        {
            CollectionId = "test-collection-id",
            CollectionName = "Test Collection",
            StatusRows = [],
        };

        var responseBuilder = new StatusResponseBuilder(censusReturn);

        // Act
        OrgDetails testTrustDetails = new()
        {
            Id = "2E774B32-E4DB-445B-B915-736C777FF5A4",
            Name = "Test Trust",
            CategoryId = "010",
            Ukprn = "222334444",
            LocalAuthorityCode = "",
            EstablishmentNumber = "",
        };
        StatusResponse response = responseBuilder.BuildResponse([testTrustDetails]);

        // Assert
        response.Details.Count.ShouldBe(1);
        OrganisationResponse responseDetail = response.Details[0];
        responseDetail.Laestab.ShouldBe(string.Empty);
        responseDetail.Name.ShouldBe(testTrustDetails.Name);
        responseDetail.CategoryId.ShouldBe(testTrustDetails.CategoryId);
        responseDetail.Ukprn.ShouldBe(testTrustDetails.Ukprn);
        responseDetail.Interesting.ShouldBeFalse();
        responseDetail.Actions.Count.ShouldBe(0);
    }

    [Fact]
    public void Gracefully_handles_null_establishment_number_and_laestab()
    {
        // Arrange
        var censusReturn = new CensusReturn
        {
            CollectionId = "test-collection-id",
            CollectionName = "Test Collection",
            StatusRows = [],
        };

        var responseBuilder = new StatusResponseBuilder(censusReturn);

        // Act
        OrgDetails testTrustDetails = new()
        {
            Id = "2E774B32-E4DB-445B-B915-736C777FF5A4",
            Name = "Test Trust",
            CategoryId = "010",
            Ukprn = "222334444",
            LocalAuthorityCode = null,
            EstablishmentNumber = null,
        };

        StatusResponse response = responseBuilder.BuildResponse([testTrustDetails]);

        // Assert
        response.Details.Count.ShouldBe(1);
        OrganisationResponse responseDetail = response.Details[0];
        responseDetail.Laestab.ShouldBe(string.Empty);
    }
}
