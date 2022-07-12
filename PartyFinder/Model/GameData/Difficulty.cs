using Newtonsoft.Json;

namespace PartyFinder.Model.GameData;

public class Difficulty
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("id")]
    public int? Id { get; set; }
}
