using Newtonsoft.Json;

namespace PartyFinder.Model.GameData;

public class Errors
{
    [JsonProperty("message")]
    public string? Message { get; set; }
}
