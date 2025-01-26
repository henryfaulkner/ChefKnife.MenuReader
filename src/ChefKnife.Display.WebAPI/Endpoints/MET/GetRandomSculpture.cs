using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ChefKnife.HttpService;
using ChefKnife.HttpService.ApiResponse;
using ChefKnife.Display.WebAPI.DTOs.MET;

namespace ChefKnife.Display.WebAPI.Endpoints.MET;

public class GetRandomSculpture : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ApiResponse<MetObject?>>
{
    readonly IHttpService _httpService;

    public GetRandomSculpture(IHttpService httpService)
    {
        _httpService = httpService;
    }

    [HttpGet(ApiEndpoints.MET.GetRandomSculpture)]
    [SwaggerOperation(
        Summary = ".",
        Description = ".",
        OperationId = "GetRandomSculpture",
        Tags = new[] { "MET" }
    )]
    public override async Task<ActionResult<ApiResponse<MetObject?>>> HandleAsync(
        CancellationToken cancellationToken = default)
    {
        var metCollectionBaseUrl = "https://collectionapi.metmuseum.org/public/collection/v1/search";
        var metObjectBaseUrl = "https://collectionapi.metmuseum.org/public/collection/v1/objects";

        try
        {

            var collection = await _httpService.GetAsync<MetCollection>($"{metCollectionBaseUrl}?medium=Sculpture&dateBegin=1950&q=*");

            if (collection?.Data?.ObjectIds == null)
            {
                throw new Exception("MET Collection response provided null data array.");
            }

            ApiResponse<MetObject?>? res = null;
            Random random = new Random();
            while (res == null && collection.Data.ObjectIds.Any())
            {
                var randIndex = random.Next(0, collection.Data.ObjectIds.Count);

                var obj = await _httpService.GetAsync<MetObject>($"{metObjectBaseUrl}/{randIndex}");

                if (obj?.Data != null 
                    && !string.IsNullOrWhiteSpace(obj.Data.PrimaryImage)
                    && obj.Data.IsPublicDomain)
                {
                    res = obj;
                }
                else
                    collection.Data.ObjectIds.RemoveAt(randIndex);
            }

            if (res == null)
            {
                throw new Exception("Failed to get response.");
            }

            return this.ToActionResult(Result.Success(res));
        }
        catch (Exception ex)
        {
            return this.ToActionResult(Result.Error($"Exception occurred: {ex.Message}"));
        }
    }
}
