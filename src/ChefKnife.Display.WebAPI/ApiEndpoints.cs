namespace ChefKnife.Display.WebAPI;

internal class ApiEndpoints
{
    public class MET
    {
        public const string GetRandomPainting = "met/get-painting/random";
        public const string GetRandomSculpture = "met/get-sculpture/random";
    }

    public class Spotify
    {
        public const string Login = "spotify/login";
        public const string Callback = "spotify/callback";
        public const string RefreshToken = "spotify/refresh-token";
        public const string Logout = "spotify/logout";

        public const string GetCurrentTrack = "spotify/current-track";
    }
}
