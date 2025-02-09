using Newtonsoft.Json;

namespace ChefKnife.Display.WebAPI.DTOs.AIC;

public class AicExhibition
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("artwork_ids")]
    public int[] ArtworkIds { get; set; }
}