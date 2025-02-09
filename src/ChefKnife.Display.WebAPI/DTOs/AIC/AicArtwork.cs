using Newtonsoft.Json;

namespace ChefKnife.Display.WebAPI.DTOs.AIC;

public class AicArtwork
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("artist_title")]
    public string Artist { get; set; }

    [JsonProperty("image_id")]
    public Guid? ImageId { get; set; }

    public string ImageUrl { get; set; }
}