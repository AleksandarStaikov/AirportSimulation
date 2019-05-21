using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AirportSimulation.App.Infrastructure;
using AirportSimulation.Common;
using AirportSimulation.Common.Models;

namespace AirportSimulation.App.Models
{
    internal class SingleCellBuildingComponent : GenericBuildingComponent
    {
        public GenericBuildingComponent Predecessor { get; set; }

        public SingleCellBuildingComponent(BuildingComponentType type, string nodeId, (int, int) cell) 
            : base(type, nodeId, cell)
        {
            
        }

        //protected override void PopulatePossibleNeighbours()
        //{
        //    var (x, y) = Cell;
        //    GridCell gridCell = null;

        //    if (x > 0)
        //    {
        //        gridCell = GetBlinkingCell((x - 1, y));
        //        PossibleNeighbours.Add(gridCell); //Top rectangle
        //    }

        //    if (SimulationGridOptions.GRID_MAX_COLUMNS > y + 1)
        //    {
        //        gridCell = GetBlinkingCell((x, y + 1));
        //        PossibleNeighbours.Add(gridCell); //Right rectangle
        //    }

        //    if (SimulationGridOptions.GRID_MAX_ROWS > x + 1)
        //    {
        //        gridCell = GetBlinkingCell((x + 1, y));
        //        PossibleNeighbours.Add(gridCell); //Bottom rectangle
        //    }

        //    if (y > 0)
        //    {
        //        gridCell = GetBlinkingCell((x, y - 1));
        //        PossibleNeighbours.Add(gridCell); //Left rectangle
        //    }
        //}

        //private GridCell GetBlinkingCell((int, int) cell)
        //{
        //    return new BlinkingCell(this, cell)
        //    {
        //        UIElement = RectangleFactory.CreateBlinkingRectangle()
        //    };
        //}
    }
}
