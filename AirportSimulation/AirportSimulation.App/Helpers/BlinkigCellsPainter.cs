using AirportSimulation.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AirportSimulation.App.Helpers
{
    class BlinkigCellsPainter
    {
        private static Grid _grid;

        public BlinkigCellsPainter(Grid grid)
        {
            _grid = grid;
        }

        public static void PopulateBlinkingCells(List<BlinkingCell> blinkingCells)
        {
            foreach(BlinkingCell blinkingCell in blinkingCells)
            {
                int index = GridHelper.CalculateIndexFromCoordinates(blinkingCell.Cell);
                MutantRectangle rectangleAtIndex = (_grid.Children[index] as MutantRectangle);

                if (rectangleAtIndex.Content.GetType() != typeof(GenericBuildingComponent))
                {
                    rectangleAtIndex.Content = blinkingCell;
                    rectangleAtIndex.Fill = blinkingCell.UIElement.Fill;
                }
            }
        }
    }
}
