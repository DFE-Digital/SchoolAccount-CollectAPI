using System.ComponentModel.DataAnnotations;
using SchoolAccount.Collect.Application.Status.GetStatuses;

namespace SchoolAccount.Collect.Api.Endpoints.Status.GetStatuses;

public record GetStatusesRequest
{
    [Required]
    public string Id { get; init; }

    [Required]
    [EmailAddress]
    public string Email { get; init; }

    [Required]
    public List<Organisation> Organisations { get; init; }

    public GetStatusesQuery ToQuery()
    {
        var orgDetails = Organisations
            .Select(x => new OrgDetails
            {
                Id = x.Id,
                Name = x.Name,
                CategoryId = x.Category.Id,
                Ukprn = x.Ukprn,
                LocalAuthorityCode = x.LocalAuthority?.Code,
                EstablishmentNumber = x.EstablishmentNumber,
            })
            .ToList();

        var requestModel = new GetStatusesRequestModel { OrgDetails = orgDetails };

        return new GetStatusesQuery(requestModel);
    }
}
