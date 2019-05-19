using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using AirportSimulation.App.Infrastructure;
using AirportSimulation.Common;
using AirportSimulation.Common.Models;

namespace AirportSimulation.App.Models
{
    internal class SingleCellBuildingComponent : GenericBuildingComponent
    {
        public SingleCellBuildingComponent(BuildingComponentType type, string nodeId, (int, int) cell) 
            : base(type, nodeId, cell)
        {
            PopulatePossibleNeighbours();
        }

        protected override void PopulatePossibleNeighbours()
        {
            var (x, y) = Cell;
            GridCell gridCell = null;

            if (x > 0)
            {
                gridCell = GetCell((x - 1, y));
                PossibleNeighbours.Add(gridCell); //Top rectangle
            }

            if (SimulationGridOptions.GRID_MAX_COLUMNS > y + 1)
            {
                gridCell = GetCell((x, y + 1));
                PossibleNeighbours.Add(gridCell); //Right rectangle
            }

            if (SimulationGridOptions.GRID_MAX_ROWS > x + 1)
            {
                gridCell = GetCell((x + 1, y));
                PossibleNeighbours.Add(gridCell); //Bottom rectangle
            }

            if (y > 0)
            {
                gridCell = GetCell((x, y - 1));
                PossibleNeighbours.Add(gridCell); //Left rectangle
            }
        }

        private GridCell GetCell((int, int) cell)
        {
            return new GridCell(cell)
            {
                UIElement = RectangleFactory.CreateBlinkingRectangle()
            };
        }
    }
}
