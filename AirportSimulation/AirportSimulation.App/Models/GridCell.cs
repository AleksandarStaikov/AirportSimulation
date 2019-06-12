namespace AirportSimulation.App.Models
{
    using System.Windows.Media;
    
    internal abstract class GridCell
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
