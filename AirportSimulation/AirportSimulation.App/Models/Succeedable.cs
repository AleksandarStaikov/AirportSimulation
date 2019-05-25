namespace AirportSimulation.App.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AirportSimulation.Common.Models;
    using AirportSimulation.App.Infrastructure;
    using AirportSimulation.App.Helpers;
    using System.Windows.Controls;

    class Succeedable : ISucceedable //TODO: Delete Succeedable
    {
        private GenericBuildingComponent _succeedableComponent;

        public Succeedable(IParent component)
        {
            _succeedableComponent = component as GenericBuildingComponent;
        }



        private void MutateAdjacentRectangle(Grid grid, (int row, int col) cell)
        {
            var index = GridHelper.CalculateIndexFromCoordinates(cell);
            var currentRectangle = grid.Children[index] as MutantRectangle;
            if(currentRectangle.Content.GetType() == typeof(EnabledCell))
            {
                _succeedableComponent.PossibleNeighbours.Add(currentRectangle);
            }

            
        }

        public void HideBlinkingCells()
        {
            foreach (MutantRectangle adjancentRectangle in _succeedableComponent.PossibleNeighbours)
            {
                var cell = adjancentRectangle.Cell;
                adjancentRectangle.ChangeContent(new DisabledCell(_succeedableComponent as IParent, cell));
            }
        }

        public void ShowBlinkingCells()
        {
            foreach (MutantRectangle adjancentRectangle in _succeedableComponent.PossibleNeighbours)
            {
                var cell = adjancentRectangle.Cell;
                adjancentRectangle.ChangeContent(new BlinkingCell(_succeedableComponent as IParent, cell));
            }
        }

        public void PopulateAdjacentRectangles(MutantRectangle parentContainer)
        {
            var (row, column) = parentContainer.Cell;
            var grid = parentContainer.GetGrid();

            if (row > 0)
            {
                MutateAdjacentRectangle(grid, (row - 1, column)); //Top rectangle
            }

            if (grid.ColumnDefinitions.Count - 1 > column)
            {
                MutateAdjacentRectangle(grid, (row, column + 1)); //Left rectangle
            }

            if (grid.RowDefinitions.Count - 1 > row)
            {
                MutateAdjacentRectangle(grid, (row + 1, column)); //Bottom rectangle
            }

            if (column > 0)
            {
                MutateAdjacentRectangle(grid, (row, column - 1)); //Right rectangle
            }

            ShowBlinkingCells();
        }
    }
}
