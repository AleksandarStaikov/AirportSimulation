namespace AirportSimulation.App.Models
{
	using System.Collections.Generic;
	using System.Linq;
	using Common;
	using Common.Models;
	using Helpers;

	internal abstract class GenericBuildingComponent : GridCell, ICreatable
    {
        public BuildingComponentType Type { get; }

        public string NodeId { get; set; }

        public readonly List<MutantRectangle> PossibleNeighbours;

        public List<BuildingComponentType> AllowedNonConveyorSuccessors { get; protected set; }

        protected ISucceedable successorEnabler;

        public List<GenericBuildingComponent> NextNodes { get; protected set; }

		protected GenericBuildingComponent(BuildingComponentType type, (int, int) cell) : base(cell)
        {
            Type = type;

            PossibleNeighbours = new List<MutantRectangle>();
            NextNodes = new List<GenericBuildingComponent>();
        }

        public virtual NodeCreationData GetCreationData()
        {
            NodeCreationData nodeData = null;
            if (!ConvertToSettingsService.ListedForCreation.Contains(NodeId))
            {
                ConvertToSettingsService.ListedForCreation.Add(NodeId);
                nodeData = new NodeCreationData
                {
                    Id = NodeId,
                    Type = Type,
                    Cell = Cell
                };

                Dictionary<NodeCreationData, int?> nextNodesData = new Dictionary<NodeCreationData, int?>();

                int? index = null;

                foreach (ICreatable nextNode in NextNodes)
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
                nodeData = ConvertToSettingsService.NodesCreationData.FirstOrDefault(data => data.Id == NodeId);
            }

            return nodeData;
        }

        public virtual NodeCreationData GetSerializedData()
        {
            NodeCreationData nodeData;

            if (!ConvertToSettingsService.ListedForSerialization.Contains(NodeId))
            {
                nodeData = new NodeCreationData
                {
                    Id = NodeId,
                    Type = Type,
                    Cell = Cell
                };

                var nextNodesData = new Dictionary<NodeCreationData, int?>();
                int? index = null;

                foreach (ICreatable nextNode in NextNodes)
                {
                    if (nextNode is ManyToOneCell manyToOne)
                    {
                        index = manyToOne.Index;
                    }

                    nextNodesData.Add(nextNode.GetSerializedData(), index ?? null);
                }

                nodeData.NextNodes = nextNodesData;
                ConvertToSettingsService.NodesSerializedData.Add(nodeData);
                ConvertToSettingsService.ListedForSerialization.Add(NodeId);
            }
            else
            {
                nodeData = ConvertToSettingsService.NodesSerializedData.FirstOrDefault(data => data.Id == NodeId);
            }

            return nodeData;
        }
    }
}
 