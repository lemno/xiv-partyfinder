using System.Collections.Generic;
using Newtonsoft.Json;

namespace PartyFinder.Model.GameData;

public class WorldData
{
    [JsonProperty("expansions")]
    public List<Expansion>? Expansions { get; set; }
}
