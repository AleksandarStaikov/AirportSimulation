namespace AirportSimulation.App.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AirportSimulation.Common;
    using AirportSimulation.App.Infrastructure;
    using AirportSimulation.Common.Models;
    using AirportSimulation.App.Helpers;
    using System.Windows.Media;

    internal class CheckIn : SingleCellBuildingComponent, ISucceedable
    {
        public CheckIn(string nodeId, (int, int) cell) : base(BuildingComponentType.CheckIn, nodeId, cell)
        {
            AllowedSuccessors = new List<BuildingComponentType>()
            {
                BuildingComponentType.PSC,
            };

            PopulateBlinkingCells();
        }

        public override void ClickHandler(MutantRectangle sender, BuildingComponentType type)
        {
            
        }

        private BlinkingCell GetBlinkingCell((int, int) cell)
        {
            return new BlinkingCell(this, cell);
        }

        public void PopulateBlinkingCells()
        {
            var (x, y) = Cell;
            BlinkingCell gridCell = null;

            if (x > 0)
            {
                gridCell = GetBlinkingCell((x - 1, y));
                PossibleNeighbours.Add(gridCell); //Top rectangle
            }

            if (SimulationGridOptions.GRID_MAX_COLUMNS > y + 1)
            {
                gridCell = GetBlinkingCell((x, y + 1));
                PossibleNeighbours.Add(gridCell); //Right rectangle
            }

            if (SimulationGridOptions.GRID_MAX_ROWS > x + 1)
            {
                gridCell = GetBlinkingCell((x + 1, y));
                PossibleNeighbours.Add(gridCell); //Bottom rectangle
            }

            if (y > 0)
            {
                gridCell = GetBlinkingCell((x, y - 1));
                PossibleNeighbours.Add(gridCell); //Left rectangle
            }

            BlinkigCellsPainter.PopulateBlinkingCells(PossibleNeighbours);
        }
    }
}
