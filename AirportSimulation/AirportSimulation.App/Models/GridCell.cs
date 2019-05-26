namespace AirportSimulation.App.Models
{
    using AirportSimulation.Common;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;

    public abstract class GridCell
    {
        //public Rectangle UIElement { get; set; }

        public Brush Fill { get; set; }

        public (int Row, int Column) Cell { get; set; }

        public GridCell((int, int) cell)
        {
            Cell = cell;
        }
    }
}
