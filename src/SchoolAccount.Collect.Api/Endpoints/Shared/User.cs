using System.ComponentModel.DataAnnotations;
using SchoolAccount.Collect.Api.Endpoints.Status.GetStatuses;
using SchoolAccount.Collect.Application.Shared;
using SchoolAccount.Collect.Application.Status.GetStatuses;

namespace SchoolAccount.Collect.Api.Endpoints.Shared;

public record User
{
    [Required]
    public string Id { get; init; }

    [Required]
    [EmailAddress]
    public string Email { get; init; }

    [Required]
    public List<Organisation> Organisations { get; init; } = [];

    public UserDetails ToUserDetails()
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

        return new UserDetails
        {
            Id = Id,
            Email = Email,
            OrgDetails = orgDetails,
        };
    }
}
