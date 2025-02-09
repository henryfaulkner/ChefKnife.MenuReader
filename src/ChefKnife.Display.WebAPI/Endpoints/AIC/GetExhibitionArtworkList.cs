using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ChefKnife.HttpService;
using ChefKnife.HttpService.ApiResponse;
using ChefKnife.Display.WebAPI.DTOs.AIC;

namespace ChefKnife.Display.WebAPI.Endpoints.AIC;

public class GetExhibitionArtworkList_Request
{
    [FromRoute]
    public int ExhibitionId { get; set; }
    [FromQuery]
    public int? Page { get; set; }
}

public class GetExhibitionArtworkList : EndpointBaseAsync
    .WithRequest<GetExhibitionArtworkList_Request>
    .WithActionResult<ApiResponse<AicResponse<IEnumerable<AicArtwork>>?>>
{
    readonly IHttpService _httpService;

    public GetExhibitionArtworkList(IHttpService httpService)
    {
        _httpService = httpService;
        _httpService.AddHeader("AIC-User-Agent", "aic-bash (chefknifestudios@gmail.com)");
    }

    [HttpGet(ApiEndpoints.AIC.GetExhibitionArtworkList)]
    [SwaggerOperation(
        Summary = ".",
        Description = ".",
        OperationId = "GetExhibitionArtworkList",
        Tags = new[] { "Art Institute of Chicago" }
    )]
    public override async Task<ActionResult<ApiResponse<AicResponse<IEnumerable<AicArtwork>>?>>> HandleAsync(
        GetExhibitionArtworkList_Request req,
        CancellationToken cancellationToken = default)
    {
        var aicExhibitionUrl = $"https://api.artic.edu/api/v1/exhibitions/{req.ExhibitionId}?fields=id,title,artwork_ids&limit=10";
        var aicArtworkUrl = "https://api.artic.edu/api/v1/artworks?fields=id,title,artist_title,image_id";

        if (req.Page.HasValue)
            aicArtworkUrl = $"{aicArtworkUrl}&page={req.Page}";

        try
        {
            ApiResponse<AicResponse<AicExhibition>?> exhibitionRes = await _httpService.GetAsync<AicResponse<AicExhibition>?>(aicExhibitionUrl);
            if (exhibitionRes?.Data == null) return this.ToActionResult(Result.Error($"Exhibition Response returned null."));
            var exhibition = exhibitionRes.Data.Data;
            if (exhibition == null) return this.ToActionResult(Result.Error($"Exhibition returned null."));

            string artworkIdCSV = string.Join(",", exhibition.ArtworkIds);
            aicArtworkUrl = $"{aicArtworkUrl}&ids={artworkIdCSV}";

            ApiResponse<AicResponse<IEnumerable<AicArtwork>>?> artworkRes = await _httpService.GetAsync<AicResponse<IEnumerable<AicArtwork>>>(aicArtworkUrl);

            if (artworkRes == null) return this.ToActionResult(Result.Error($"Artwork Response returned null."));

            if (artworkRes.Data?.Data != null)
            {
                foreach (var artwork in artworkRes.Data.Data)
                {
                    if (artwork.ImageId.HasValue)
                        artwork.ImageUrl = FormatImageUrl(artwork.ImageId.Value);
                }
            }

            return this.ToActionResult(Result.Success(artworkRes));
        }
        catch (Exception ex)
        {
            return this.ToActionResult(Result.Error($"Exception occurred: {ex.Message}"));
        }
    }

    static string FormatImageUrl(Guid imageId)
    {
        return $"https://www.artic.edu/iiif/2/{imageId.ToString()}/full/843,/0/default.jpg";
    }
}