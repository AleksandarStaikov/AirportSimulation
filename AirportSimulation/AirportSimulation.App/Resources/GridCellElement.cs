namespace AirportSimulation.App.Resources
{
    using System.Collections.Generic;
    using System.Windows;

    public class GridCellElement
    {
        public UIElement Element { get; set; }

        public KeyValuePair<int, int> Cell { get; set; }
    }
}
