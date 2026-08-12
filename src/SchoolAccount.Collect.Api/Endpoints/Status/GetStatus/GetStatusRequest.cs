using System.ComponentModel.DataAnnotations;

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
}
