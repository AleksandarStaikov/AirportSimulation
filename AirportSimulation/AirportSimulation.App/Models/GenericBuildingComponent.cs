using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportSimulation.Common;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using AirportSimulation.App.Infrastructure;
using System.Windows;
using AirportSimulation.App.Helpers;
using AirportSimulation.Common.Models;

namespace AirportSimulation.App.Models
{
    internal abstract class GenericBuildingComponent : GridCell, ICreatable
    {
        public BuildingComponentType Type { get; }

        public string NodeId { get; }

        public readonly List<MutantRectangle> PossibleNeighbours;

        public List<BuildingComponentType> AllowedNonConveyorSuccessors { get; protected set; }

        protected ISucceedable successorEnabler;

        public List<GenericBuildingComponent> NextNodes { get; protected set; }

        public GenericBuildingComponent(BuildingComponentType type, (int, int) cell) : base(cell)
        {
            Type = type;
            NodeId = Guid.NewGuid().ToString();

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
            }
            else
            {
                nodeData = ConvertToSettingsService.NodesCreationData.FirstOrDefault(data => data.Id == this.NodeId);
            }

            ConvertToSettingsService.NodesCreationData.Add(nodeData);
            return nodeData;
        }
    }
}
 