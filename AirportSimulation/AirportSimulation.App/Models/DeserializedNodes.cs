using Newtonsoft.Json;
using System.Collections.Generic;

namespace AirportSimulation.App.Models
{
    [JsonArray]
    public class DeserializedNodes
    {
        [JsonIgnore]
        public List<object> ChildrenTokens { get; set; } = new List<object>();
    }
}
