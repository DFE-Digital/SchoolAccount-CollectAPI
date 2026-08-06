using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SchoolAccount.Collect.Api.Endpoints.Organisations.GetByLaestab;

public sealed record GetByLaestabRequest
{
    [FromRoute]
    [RegularExpression(
        @"^\d{7}$",
        ErrorMessage = "LAESTAB identifiers are 7 character numeric only values in the format 1234567"
    )]
    [Description("LAESTAB of the organisation. Must be a seven digit number.")]
    public string Laestab { get; init; }
};
