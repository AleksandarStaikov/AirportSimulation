namespace AirportSimulation.Common.Models
{
    using System.Collections.Generic;

    public class NodeCreationData
    {
        public string Id { get; set; }

        public Dictionary<NodeCreationData, int?> NextNodes { get; set; }

        public BuildingComponentType Type { get; set; }

        public int? Length { get; set; }

        public int? ConveyorIndex { get; set; }
    }
}
