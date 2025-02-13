using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using ChefKnife.HttpService.ApiResponse;
using ChefKnife.HttpService;
using Swashbuckle.AspNetCore.Annotations;
using Ardalis.Result.AspNetCore;
using Ardalis.Result;
using Newtonsoft.Json;

namespace ChefKnife.Display.WebAPI.Endpoints.Spotify;

public record RefreshTokenRequest
{
    public required string RefreshToken { get; set; }
}

public record RefreshTokenResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("token_type")]
    public string TokenType { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonProperty("scope")]
    public string Scope { get; set; }
}

public class RefreshToken : EndpointBaseAsync
    .WithRequest<RefreshTokenRequest>
    .WithActionResult<ApiResponse<RefreshTokenResponse>>
{
    readonly IHttpService _httpService;
    readonly IConfiguration _config;

    public RefreshToken(IHttpService httpService, IConfiguration config)
    {
        _httpService = httpService;
        _config = config;

        _httpService.SetBasicAuthentication(config["Spotify:ClientId"], config["Spotify:ClientSecret"]);
    }

    [HttpPost(ApiEndpoints.Spotify.RefreshToken)]
    [SwaggerOperation(
        Summary = "Refresh the Spotify Access Token",
        Description = "Uses a refresh token to obtain a new access token for Spotify API.",
        OperationId = "RefreshToken",
        Tags = new[] { "Spotify" }
    )]
    public override async Task<ActionResult<ApiResponse<RefreshTokenResponse?>>> HandleAsync(
        [FromBody] RefreshTokenRequest req,
        CancellationToken cancellationToken)
    {
        try
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", req.RefreshToken),
                new KeyValuePair<string, string>("client_id", _config["Spotify:ClientId"]),
            });

            var res = await _httpService.PostAsync<RefreshTokenResponse>("https://accounts.spotify.com/api/token", content);
            if (res.Data == null) throw new Exception("Spotify returned a null response.");

            res.Data.RefreshToken = res.Data.RefreshToken ?? req.RefreshToken;

            return this.ToActionResult(Result.Success(res));
        }
        catch (Exception ex)
        {
            return this.ToActionResult(Result.Error($"Exception occurred: {ex.Message}"));
        }
    }
}