using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Collect.Api.Endpoints.Shared;
using SchoolAccount.Collect.Application.Census.GetCensusActions;

namespace SchoolAccount.Collect.Api.Endpoints.Census.GetCensusActions;

public class GetCensusActionsRequest
{
    [FromRoute]
    [Required]
    public string CensusId { get; init; }

    [FromBody]
    public User User { get; init; }

    public GetCensusActionsQuery ToQuery()
    {
        var requestModel = new GetCensusActionsRequestModel
        {
            CensusId = CensusId,
            UserDetails = User.ToUserDetails(),
        };

        return new GetCensusActionsQuery(requestModel);
    }
}
