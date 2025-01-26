using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ChefKnife.Display.WebAPI.DTOs.Spotify;

public class DeviceInfo
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("is_active")]
    public bool IsActive { get; set; }

    [JsonProperty("is_private_session")]
    public bool IsPrivateSession { get; set; }

    [JsonProperty("is_restricted")]
    public bool IsRestricted { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("volume_percent")]
    public int VolumePercent { get; set; }

    [JsonProperty("supports_volume")]
    public bool SupportsVolume { get; set; }
}

public class ExternalUrls
{
    [JsonProperty("spotify")]
    public string Spotify { get; set; }
}

public class Image
{
    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("height")]
    public int Height { get; set; }

    [JsonProperty("width")]
    public int Width { get; set; }
}

public class Restrictions
{
    [JsonProperty("reason")]
    public string Reason { get; set; }
}

public class Artist
{
    [JsonProperty("external_urls")]
    public ExternalUrls ExternalUrls { get; set; }

    [JsonProperty("href")]
    public string Href { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("uri")]
    public string Uri { get; set; }
}

public class Album
{
    [JsonProperty("album_type")]
    public string AlbumType { get; set; }

    [JsonProperty("total_tracks")]
    public int TotalTracks { get; set; }

    [JsonProperty("available_markets")]
    public List<string> AvailableMarkets { get; set; }

    [JsonProperty("external_urls")]
    public ExternalUrls ExternalUrls { get; set; }

    [JsonProperty("href")]
    public string Href { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("images")]
    public List<Image> Images { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("release_date")]
    public string ReleaseDate { get; set; }

    [JsonProperty("release_date_precision")]
    public string ReleaseDatePrecision { get; set; }

    [JsonProperty("restrictions")]
    public Restrictions Restrictions { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("uri")]
    public string Uri { get; set; }

    [JsonProperty("artists")]
    public List<Artist> Artists { get; set; }
}

public class ExternalIds
{
    [JsonProperty("isrc")]
    public string Isrc { get; set; }

    [JsonProperty("ean")]
    public string Ean { get; set; }

    [JsonProperty("upc")]
    public string Upc { get; set; }
}

public class Track
{
    [JsonProperty("album")]
    public Album Album { get; set; }

    [JsonProperty("artists")]
    public List<Artist> Artists { get; set; }

    [JsonProperty("available_markets")]
    public List<string> AvailableMarkets { get; set; }

    [JsonProperty("disc_number")]
    public int DiscNumber { get; set; }

    [JsonProperty("duration_ms")]
    public int DurationMs { get; set; }

    [JsonProperty("explicit")]
    public bool Explicit { get; set; }

    [JsonProperty("external_ids")]
    public ExternalIds ExternalIds { get; set; }

    [JsonProperty("external_urls")]
    public ExternalUrls ExternalUrls { get; set; }

    [JsonProperty("href")]
    public string Href { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("is_playable")]
    public bool IsPlayable { get; set; }

    [JsonProperty("linked_from")]
    public object LinkedFrom { get; set; }

    [JsonProperty("restrictions")]
    public Restrictions Restrictions { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("popularity")]
    public int Popularity { get; set; }

    [JsonProperty("preview_url")]
    public string PreviewUrl { get; set; }

    [JsonProperty("track_number")]
    public int TrackNumber { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("uri")]
    public string Uri { get; set; }

    [JsonProperty("is_local")]
    public bool IsLocal { get; set; }
}

public class Context
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("href")]
    public string Href { get; set; }

    [JsonProperty("external_urls")]
    public ExternalUrls ExternalUrls { get; set; }

    [JsonProperty("uri")]
    public string Uri { get; set; }
}

public class Actions
{
    [JsonProperty("interrupting_playback")]
    public bool InterruptingPlayback { get; set; }

    [JsonProperty("pausing")]
    public bool Pausing { get; set; }

    [JsonProperty("resuming")]
    public bool Resuming { get; set; }

    [JsonProperty("seeking")]
    public bool Seeking { get; set; }

    [JsonProperty("skipping_next")]
    public bool SkippingNext { get; set; }

    [JsonProperty("skipping_prev")]
    public bool SkippingPrev { get; set; }

    [JsonProperty("toggling_repeat_context")]
    public bool TogglingRepeatContext { get; set; }

    [JsonProperty("toggling_shuffle")]
    public bool TogglingShuffle { get; set; }

    [JsonProperty("toggling_repeat_track")]
    public bool TogglingRepeatTrack { get; set; }

    [JsonProperty("transferring_playback")]
    public bool TransferringPlayback { get; set; }
}

public class CurrentlyPlaying
{
    [JsonProperty("device")]
    public DeviceInfo Device { get; set; }

    [JsonProperty("repeat_state")]
    public string RepeatState { get; set; }

    [JsonProperty("shuffle_state")]
    public bool ShuffleState { get; set; }

    [JsonProperty("context")]
    public Context Context { get; set; }

    [JsonProperty("timestamp")]
    public long Timestamp { get; set; }

    [JsonProperty("progress_ms")]
    public int ProgressMs { get; set; }

    [JsonProperty("is_playing")]
    public bool IsPlaying { get; set; }

    [JsonProperty("item")]
    public Track Item { get; set; }

    [JsonProperty("currently_playing_type")]
    public string CurrentlyPlayingType { get; set; }

    [JsonProperty("actions")]
    public Actions Actions { get; set; }
}
