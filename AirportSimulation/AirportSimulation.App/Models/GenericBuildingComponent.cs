namespace AirportSimulation.App.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AirportSimulation.Common;
    using AirportSimulation.App.Helpers;
    using AirportSimulation.Common.Models;
    
    internal abstract class GenericBuildingComponent : GridCell, ICreatable
    {
        public BuildingComponentType Type { get; }

        public string NodeId { get; set; }

        public readonly List<MutantRectangle> PossibleNeighbours;

        public List<BuildingComponentType> AllowedNonConveyorSuccessors { get; protected set; }

        protected ISucceedable successorEnabler;

        public List<GenericBuildingComponent> NextNodes { get; protected set; }

        public GenericBuildingComponent(BuildingComponentType type, (int, int) cell) : base(cell)
        {
            Type = type;

            PossibleNeighbours = new List<MutantRectangle>();
            NextNodes = new List<GenericBuildingComponent>();
        }

        public virtual NodeCreationData GetCreationData()
        {
            NodeCreationData nodeData = null;
            if (!ConvertToSettingsService.Listed.Contains(this.NodeId))
            {
                ConvertToSettingsService.Listed.Add(this.NodeId);
                nodeData = new NodeCreationData
                {
                    Id = this.NodeId,
                    Type = this.Type
                };
                Dictionary<NodeCreationData, int?> nextNodesData = new Dictionary<NodeCreationData, int?>();

                int? index = null;

                foreach (ICreatable nextNode in this.NextNodes)
                {
                    if (nextNode is ManyToOneCell manyToOne)
                    {
                        index = manyToOne.Index;
                    }
                    nextNodesData.Add(nextNode.GetCreationData(), index ?? null);
                }

                nodeData.NextNodes = nextNodesData;
                ConvertToSettingsService.NodesCreationData.Add(nodeData);
            }
            else
            {
                nodeData = ConvertToSettingsService.NodesCreationData.FirstOrDefault(data => data.Id == this.NodeId);
            }

            return nodeData;
        }
    }
}
 