using System.ComponentModel.DataAnnotations;
using SchoolAccount.Collect.Application.Census.GetCensusActions;

namespace SchoolAccount.Collect.Api.Endpoints.Census.GetCensusActions;

public class GetCensusActionsRequest
{
    [Required]
    public string Id { get; init; }

    public GetCensusActionsQuery ToQuery()
    {
        var requestModel = new GetCensusActionsRequestModel { Id = Id };

        return new GetCensusActionsQuery(requestModel);
    }
}
