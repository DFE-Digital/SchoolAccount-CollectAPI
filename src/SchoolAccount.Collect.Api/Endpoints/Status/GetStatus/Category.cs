using System.ComponentModel.DataAnnotations;

namespace SchoolAccount.Collect.Api.Endpoints.Status.GetStatus;

public record Category
{
    [Required]
    public string Id { get; init; }

    [Required]
    public string Name { get; init; }
}
