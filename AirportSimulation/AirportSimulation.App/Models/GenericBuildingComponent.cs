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

namespace AirportSimulation.App.Models
{
    internal abstract class GenericBuildingComponent : GridCell
    {
        public BuildingComponentType Type { get; }

        public string NodeId { get; }

        public readonly List<MutantRectangle> PossibleNeighbours;

        public List<BuildingComponentType> AllowedNonConveyorSuccessors { get; protected set; }

        protected ISucceedable successorEnabler;

        public List<GenericBuildingComponent> NextNodes { get; protected set; }

        public GenericBuildingComponent(BuildingComponentType type, string nodeId, (int, int) cell) : base(cell)
        {
            Type = type;
            NodeId = nodeId;

            PossibleNeighbours = new List<MutantRectangle>();
            NextNodes = new List<GenericBuildingComponent>();
        }
    }
}
 