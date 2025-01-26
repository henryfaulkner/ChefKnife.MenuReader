using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;
using Ardalis.Result;
using ChefKnife.Display.WebAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ChefKnife.HttpService;
using System.Web;

namespace ChefKnife.Display.WebAPI.Endpoints.Spotify;

public class Login : EndpointBaseAsync
    .WithoutRequest
    .WithoutResult
{
    readonly IHttpService _httpService;
    readonly IConfiguration _config;

    public Login(IHttpService httpService, IConfiguration config)
    {
        _httpService = httpService;
        _config = config;
    }

    [HttpGet(ApiEndpoints.Spotify.Login)]
    [SwaggerOperation(
        Summary = ".",
        Description = ".",
        OperationId = "Login",
        Tags = new[] { "Spotify" }
    )]
    public override async Task<ActionResult> HandleAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            // Generate a random state string
            string state = GenerateRandomString(16);

            // Define the required scopes
            string scope = "user-read-currently-playing";

            // Build the authorization URL
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            queryParams["show_dialog"] = "true";
            queryParams["response_type"] = "code";
            queryParams["client_id"] = _config["Spotify:ClientId"];
            queryParams["scope"] = scope;
            queryParams["redirect_uri"] = _config["Spotify:RedirectUri"];
            queryParams["state"] = state;

            string spotifyAuthorizeUrl = $"https://accounts.spotify.com/authorize?{queryParams}";

            // Redirect to Spotify authorization
            return Redirect(spotifyAuthorizeUrl);
        }
        catch (Exception ex)
        {
            return this.ToActionResult(Result.Error($"Exception occurred: {ex.Message}"));
        }
    }

    static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var stringChars = new char[length];

        for (int i = 0; i < length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        return new string(stringChars);
    }
}
