using Newtonsoft.Json;

namespace ChefKnife.Display.WebAPI.DTOs.AIC;

public class AicResponse<T>
{
    [JsonProperty("data")]
    public T Data { get; set; } 

    [JsonProperty("info")]
    public AicResponseInfo Info { get; set; }

    [JsonProperty("config")]
    public AicResponseConfig Config { get; set; }

    [JsonProperty("pagination")]
    public Pagination Pagination { get; set; }
}

public class AicResponseInfo
{
    [JsonProperty("license_text")]
    public string LicenseText { get; set; }

    [JsonProperty("license_links")]
    public List<string> LicenseLinks { get; set; }

    [JsonProperty("version")]
    public string Version { get; set; }
}

public class AicResponseConfig
{
    [JsonProperty("iiif_url")]
    public string IiifUrl { get; set; }

    [JsonProperty("website_url")]
    public string WebsiteUrl { get; set; }
}

public class Pagination
{
    [JsonProperty("total")]
    public int Total { get; set; }

    [JsonProperty("limit")]
    public int Limit { get; set; }

    [JsonProperty("offset")]
    public int Offset { get; set; }

    [JsonProperty("total_pages")]
    public int TotalPages { get; set; }

    [JsonProperty("current_page")]
    public int CurrentPage { get; set; }

    [JsonProperty("next_url")]
    public string NextUrl { get; set; }
}