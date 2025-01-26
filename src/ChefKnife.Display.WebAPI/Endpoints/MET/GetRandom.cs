using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;
using Ardalis.Result;
using ChefKnife.Display.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ChefKnife.HttpService;
using ChefKnife.HttpService.ApiResponse;

namespace ChefKnife.Display.WebAPI.Endpoints.MET;

public class GetRandom : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<MetObject>
{
    readonly IHttpService _httpService;

    public GetRandom(IHttpService httpService)
    {
        _httpService = httpService;
    }

    [HttpGet(ApiEndpoints.MET.GetRandom)]
    [SwaggerOperation(
        Summary = ".",
        Description = ".",
        OperationId = "GetRandom",
        Tags = new[] { "Display", "MET" }
    )]
    public override async Task<ActionResult<MetObject>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        var metCollectionBaseUrl = "https://collectionapi.metmuseum.org/public/collection/v1/search";
        var metObjectBaseUrl = "https://collectionapi.metmuseum.org/public/collection/v1/objects";

        try
        {

            var collection = await _httpService.GetAsync<MetCollection>($"{metCollectionBaseUrl}?medium=Paintings&q=*");

            if (collection?.Data?.ObjectIds == null)
            {
                throw new Exception("MET Collection response provided null data array.");
            }

            MetObject? res = null;
            Random random = new Random();
            while (res == null && collection.Data.ObjectIds.Any())
            {
                var randIndex = random.Next(0, collection.Data.ObjectIds.Count);

                var obj = await _httpService.GetAsync<MetObject>($"{metObjectBaseUrl}/{randIndex}");

                if (obj?.Data != null 
                    && !string.IsNullOrWhiteSpace(obj.Data.PrimaryImage)
                    && obj.Data.IsPublicDomain)
                {
                    res = obj.Data;
                }
                else
                    collection.Data.ObjectIds.RemoveAt(randIndex);
            }


            return this.ToActionResult(Result.Success(res));
        }
        catch (Exception ex)
        {
            return this.ToActionResult(Result.Error($"Exception occurred: {ex.Message}"));
        }
    }
}
