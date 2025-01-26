using Newtonsoft.Json;

namespace ChefKnife.Display.WebAPI.DTOs.MET;

public class MetCollection
{
    [JsonProperty("total")]
    public int Total { get; set; }

    [JsonProperty("objectIDs")]
    public List<long> ObjectIds { get; set; }
}
