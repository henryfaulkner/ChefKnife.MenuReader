using Newtonsoft.Json;

namespace ChefKnife.Display.WebAPI.DTOs;

public class MetCollection
{
    [JsonProperty("total")] 
    public int Total { get; set; }

    [JsonProperty("objectIDs")] 
    public List<long> ObjectIds { get; set; }
}
