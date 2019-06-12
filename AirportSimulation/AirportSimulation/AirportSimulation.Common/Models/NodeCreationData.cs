namespace AirportSimulation.Common.Models
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class NodeCreationData
    {
        public string Id { get; set; }

        public Dictionary<NodeCreationData, int?> NextNodes { get; set; }

        public (int Row, int Column) Cell { get; set; }

        public BuildingComponentType Type { get; set; }

        public int? Length { get; set; }

        public int? ConveyorIndex { get; set; }
    }
}
