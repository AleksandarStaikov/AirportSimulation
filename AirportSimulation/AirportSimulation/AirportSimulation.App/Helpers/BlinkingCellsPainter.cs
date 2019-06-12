namespace AirportSimulation.App.Helpers
{
    using AirportSimulation.App.Models;
    using System.Collections.Generic;
    using System.Windows.Controls;

    class BlinkingCellsPainter
    {
        private static Grid _grid;

        public BlinkingCellsPainter(Grid grid)
        {
            _grid = grid;
        }

        public static void PaintChildrenCells(List<GridCell> blinkingCells)
        {
            foreach(GridCell blinkingCell in blinkingCells)
            {
                int index = GridHelper.CalculateIndexFromCoordinates(blinkingCell.Cell);
                MutantRectangle rectangleAtIndex = (_grid.Children[index] as MutantRectangle);

                if (rectangleAtIndex.Content.GetType() != typeof(GenericBuildingComponent))
                {
                    rectangleAtIndex.Content = blinkingCell;
                    rectangleAtIndex.Fill = blinkingCell.Fill;
                }
            }
        }
    }
}
