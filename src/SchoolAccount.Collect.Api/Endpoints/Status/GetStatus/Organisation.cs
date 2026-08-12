using System.ComponentModel.DataAnnotations;

namespace SchoolAccount.Collect.Api.Endpoints.Status.GetStatus;

public record Organisation
{
    [Required]
    public string Id { get; init; }

    [Required]
    public string Name { get; init; }

    [Required]
    public Category Category { get; init; }

    [Required]
    public string Ukprn { get; init; }
    public LocalAuthority? LocalAuthority { get; init; }
    public string? EstablishmentNumber { get; init; }
}
