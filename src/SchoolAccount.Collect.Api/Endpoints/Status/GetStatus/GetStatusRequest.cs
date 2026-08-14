using System.ComponentModel.DataAnnotations;
using SchoolAccount.Collect.Application.Status.GetStatus;

namespace SchoolAccount.Collect.Api.Endpoints.Status.GetStatus;

public record GetStatusRequest
{
    [Required]
    public string Id { get; init; }

    [Required]
    [EmailAddress]
    public string Email { get; init; }

    [Required]
    public List<Organisation> Organisations { get; init; }

    public GetStatusQuery ToQuery()
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

        var requestModel = new GetStatusRequestModel { OrgDetails = orgDetails };

        return new GetStatusQuery(requestModel);
    }
}
