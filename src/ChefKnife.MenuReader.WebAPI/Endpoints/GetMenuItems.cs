using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ChefKnife.MenuReader.Core.DTOs;

namespace ChefKnife.MenuReader.WebAPI.Endpoints;

public record GetMenuItemsModel
{
    [FromQuery(Name = "menuUrl")]
    public string MenuUrl { get; set; }
}

public class GetMenuItems : EndpointBaseAsync
    .WithRequest<GetMenuItemsModel>
    .WithActionResult<IEnumerable<MenuItemDetail>>
{
    [HttpGet(ApiEndpoints.GetMenuItems)]
    [SwaggerOperation(
        Summary = ".",
        Description = ".",
        OperationId = "GetMenuItems",
        Tags = new[] { "MenuReader" }
    )]
    public override async Task<ActionResult<IEnumerable<MenuItemDetail>>> HandleAsync(
        GetMenuItemsModel req,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var res = new List<MenuItemDetail>();



            return this.ToActionResult(Result.Success(res.AsEnumerable()));
        }
        catch (Exception ex)
        {
            return this.ToActionResult(Result.Error($"Exception occurred: {ex.Message}"));
        }
    }
}
