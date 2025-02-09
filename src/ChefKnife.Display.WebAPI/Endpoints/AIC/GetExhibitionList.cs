using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ChefKnife.HttpService;
using ChefKnife.HttpService.ApiResponse;
using ChefKnife.Display.WebAPI.DTOs.AIC;

namespace ChefKnife.Display.WebAPI.Endpoints.AIC;

public class GetExhibitionList_Request
{
    [FromQuery]
    public int? Page { get; set; }
}

public class GetExhibitionList : EndpointBaseAsync
    .WithRequest<GetExhibitionList_Request>
    .WithActionResult<ApiResponse<AicResponse<IEnumerable<AicExhibition>>?>>
{
    readonly IHttpService _httpService;

    public GetExhibitionList(IHttpService httpService)
    {
        _httpService = httpService;
        _httpService.AddHeader("AIC-User-Agent", "aic-bash (chefknifestudios@gmail.com)");
    }

    [HttpGet(ApiEndpoints.AIC.GetExhibitionList)]
    [SwaggerOperation(
        Summary = ".",
        Description = ".",
        OperationId = "GetExhibitionList",
        Tags = new[] { "Art Institute of Chicago" }
    )]
    public override async Task<ActionResult<ApiResponse<AicResponse<IEnumerable<AicExhibition>>?>>> HandleAsync(
        GetExhibitionList_Request req,
        CancellationToken cancellationToken = default)
    {
        var aicExhibitionUrl = $"https://api.artic.edu/api/v1/exhibitions?fields=id,title,artwork_ids&limit=100";

        if (req.Page.HasValue)
            aicExhibitionUrl = $"{aicExhibitionUrl}&page={req.Page}";

        try
        {
            ApiResponse<AicResponse<IEnumerable<AicExhibition>>?> exhibitionRes = await _httpService.GetAsync<AicResponse<IEnumerable<AicExhibition>>?>(aicExhibitionUrl);
            if (exhibitionRes?.Data == null) return this.ToActionResult(Result.Error($"Exhibition Response returned null."));

            exhibitionRes.Data.Data = exhibitionRes.Data.Data?
                .Where(x => x.ArtworkIds.Length > 0) ?? [];

            return this.ToActionResult(Result.Success(exhibitionRes));
        }
        catch (Exception ex)
        {
            return this.ToActionResult(Result.Error($"Exception occurred: {ex.Message}"));
        }
    }
}