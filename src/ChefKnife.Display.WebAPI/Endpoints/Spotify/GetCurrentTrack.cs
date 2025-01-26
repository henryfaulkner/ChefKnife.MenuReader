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
using ChefKnife.Display.WebAPI.DTOs.Spotify;

namespace ChefKnife.Display.WebAPI.Endpoints.Spotify;

public class GetCurrentTrack : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ApiResponse<CurrentlyPlaying?>>
{
    readonly IHttpService _httpService;
    readonly IConfiguration _config;

    public GetCurrentTrack(IHttpService httpService, IConfiguration config)
    {
        _httpService = httpService;
        _config = config;
    }

    [HttpGet(ApiEndpoints.Spotify.GetCurrentTrack)]
    [SwaggerOperation(
        Summary = ".",
        Description = ".",
        OperationId = "GetCurrentTrack",
        Tags = new[] { "Spotify" }
    )]
    public override async Task<ActionResult<ApiResponse<CurrentlyPlaying?>>> HandleAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            _httpService.SetBearerAuthentication(GetBearToken());

            var res = await _httpService.GetAsync<CurrentlyPlaying>("https://api.spotify.com/v1/me/player/currently-playing");

            return this.ToActionResult(Result.Success(res));
        }
        catch (Exception ex)
        {
            return this.ToActionResult(Result.Error($"Exception occurred: {ex.Message}"));
        }
    }

    string GetBearToken()
    {
        // Get the Authorization header from the request
        if (Request.Headers.ContainsKey("Authorization"))
        {
            string authorizationHeader = Request.Headers["Authorization"];

            // You can then process the header, for example, splitting it to get the Bearer token
            if (authorizationHeader.StartsWith("Bearer "))
            {
                string token = authorizationHeader.Substring("Bearer ".Length).Trim();
                return token;
            }
        }
        return string.Empty;
    }
}
