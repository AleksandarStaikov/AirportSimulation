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

namespace AirportSimulation.App.Models
{
    public abstract class GenericBuildingComponent : GridCell
    {
        public BuildingComponentType Type { get; }

        public string NodeId { get; }

        public List<GridCell> PossibleNeighbours { get; set; }

        public GenericBuildingComponent(BuildingComponentType type, string nodeId, (int, int) cell) : base(cell)
        {
            Type = type;
            NodeId = nodeId;

            PossibleNeighbours = new List<GridCell>();
        }

        protected abstract void PopulatePossibleNeighbours();
    }
}
