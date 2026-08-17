using System.ComponentModel.DataAnnotations;

namespace SchoolAccount.Collect.Api.Endpoints.Status.GetStatus;

public record LocalAuthority
{
    [Required]
    public string Id { get; init; }

    [Required]
    public string Name { get; init; }

    [Required]
    public string Code { get; init; }
}
