using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;
using Ardalis.Result;
using ChefKnife.Display.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ChefKnife.HttpService;
using System.Web;
using ChefKnife.HttpService.ApiResponse;
using Newtonsoft.Json;

namespace ChefKnife.Display.WebAPI.Endpoints.Spotify;

public record CallbackRequest
{
    [FromQuery]
    public string? Code { get; set; }

    [FromQuery]
    public string? State { get; set; }
}

public record SpotifyTokens
{
    [JsonProperty("access_token")]
    public string? AccessToken { get; set; }

    [JsonProperty("refresh_token")]
    public string? RefreshToken { get; set; }
}

public class Callback : EndpointBaseAsync
    .WithRequest<CallbackRequest>
    .WithActionResult<ApiResponse<SpotifyTokens?>>
{
    readonly IHttpService _httpService;
    readonly IConfiguration _config;

    public Callback(IHttpService httpService, IConfiguration config)
    {
        _httpService = httpService;
        _config = config;

        _httpService.SetBasicAuthentication(config["Spotify:ClientId"], config["Spotify:ClientSecret"]);
    }

    [HttpGet(ApiEndpoints.Spotify.Callback)]
    [SwaggerOperation(
        Summary = ".",
        Description = ".",
        OperationId = "Callback",
        Tags = new[] { "Spotify" }
    )]
    public override async Task<ActionResult<ApiResponse<SpotifyTokens?>>> HandleAsync(
        CallbackRequest req,
        CancellationToken cancellationToken)
    {
        try
        {
            if (req.Code == null) 
            {
                return Redirect($"{_config["ClientUrl"]}/#?error=code_mismatch");
            }

            if (req.State == null)
            {
                return Redirect($"{_config["ClientUrl"]}/#?error=state_mismatch");
            }

            // Create form data for the request
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", req.Code),
                new KeyValuePair<string, string>("redirect_uri", $"{Request.Scheme}://{Request.Host}{_config["Spotify:RedirectEndpoint"]}"),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
            });

            var res = await _httpService.PostAsync<SpotifyTokens>($"https://accounts.spotify.com/api/token", content);

            if (res?.Data == null || !res.IsSuccessful || res.StatusCode != 200)
            {
                throw new Exception(res?.Exception?.Message ?? (res?.Message ?? "Failed to get access token."));
            }

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            queryParams["access_token"] = res.Data.AccessToken;
            queryParams["refresh_token"] = res.Data.RefreshToken;

            return Redirect($"{_config["ClientUrl"]}?{queryParams}");
        }
        catch (Exception ex)
        {
            return this.ToActionResult(Result.Error($"Exception occurred: {ex.Message}"));
        }
    }
}
