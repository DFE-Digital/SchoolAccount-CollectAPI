namespace SchoolAccount.Collect.Api.Endpoints.Status.GetStatus;

public class GetStatusRequest
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required List<Organisation> Organisations { get; init; }
}
