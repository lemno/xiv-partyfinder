using Newtonsoft.Json;

namespace PartyFinder.Model.GameData;

public class Encounter
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("id")]
    public int? Id { get; set; }
}
