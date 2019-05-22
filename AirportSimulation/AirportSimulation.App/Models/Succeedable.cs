namespace AirportSimulation.App.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AirportSimulation.Common.Models;
    using AirportSimulation.App.Infrastructure;

    class Succeedable : ISucceedable //TODO: Delete Succeedable
    {
        private GenericBuildingComponent _succeedableComponent;

        public Succeedable(GenericBuildingComponent component)
        {
            _succeedableComponent = component;
        }

        private BlinkingCell GetBlinkingCell((int, int) cell)
        {
            return new BlinkingCell(_succeedableComponent, cell);
        }

        public void PopulateBlinkingCells()
        {
            var (x, y) = _succeedableComponent.Cell;
            BlinkingCell gridCell = null;

            if (x > 0)
            {
                gridCell = GetBlinkingCell((x - 1, y));
                _succeedableComponent.PossibleNeighbours.Add(gridCell); //Top rectangle
            }

            if (SimulationGridOptions.GRID_MAX_COLUMNS > y + 1)
            {
                gridCell = GetBlinkingCell((x, y + 1));
                _succeedableComponent.PossibleNeighbours.Add(gridCell); //Right rectangle
            }

            if (SimulationGridOptions.GRID_MAX_ROWS > x + 1)
            {
                gridCell = GetBlinkingCell((x + 1, y));
                _succeedableComponent.PossibleNeighbours.Add(gridCell); //Bottom rectangle
            }

            if (y > 0)
            {
                gridCell = GetBlinkingCell((x, y - 1));
                _succeedableComponent.PossibleNeighbours.Add(gridCell); //Left rectangle
            }
        }
    }
}
