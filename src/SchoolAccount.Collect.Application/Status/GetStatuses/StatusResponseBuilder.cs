using SchoolAccount.Collect.Application.Census.GetCensusActions;

namespace SchoolAccount.Collect.Application.Status.GetStatuses;

public class StatusResponseBuilder(CensusReturn censusReturn)
{
    public StatusResponse BuildResponse(List<OrgDetails> orgDetails)
    {
        return new StatusResponse
        {
            Details = orgDetails.Select(CreateOrganisationResponse).ToList(),
        };
    }

    private OrganisationResponse CreateOrganisationResponse(OrgDetails orgDetails)
    {
        bool interesting = !string.IsNullOrEmpty(orgDetails.Laestab);
        return new OrganisationResponse
        {
            Id = orgDetails.Id,
            Name = orgDetails.Name,
            CategoryId = orgDetails.CategoryId,
            Ukprn = orgDetails.Ukprn,
            Laestab = orgDetails.Laestab,
            Interesting = interesting,
            Actions = interesting ? GetActions(orgDetails.Laestab) : [],
        };
    }

    private List<Action> GetActions(string laestab)
    {
        int status = censusReturn
            .StatusRows.Where(s => s.LAEStab == laestab)
            .Select(s => s.ReturnStatusCode)
            .FirstOrDefault();
        string statusName = ReturnStatusMapper.GetStatusDescription(status);
        return
        [
            new()
            {
                Id = censusReturn.CollectionId,
                Name = censusReturn.CollectionName,
                Status = new Status { Name = statusName },
            },
        ];
    }
}
