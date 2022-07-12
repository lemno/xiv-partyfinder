using Newtonsoft.Json;

namespace PartyFinder.Model.GameData;

public class Data
{
    [JsonProperty("worldData")]
    public WorldData? WorldData { get; set; }
}
