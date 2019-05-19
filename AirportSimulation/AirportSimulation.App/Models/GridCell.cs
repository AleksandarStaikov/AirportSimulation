namespace AirportSimulation.App.Models
{
    using System.Windows;

    public class GridCell
    {
        public UIElement UIElement { get; set; }

        public (int X, int Y) Cell { get; set; }

        public GridCell((int, int) cell)
        {
            Cell = cell;
        }
    }
}
