namespace AirportSimulation.App.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    public class GridCellElement
    {
        public UIElement Element { get; set; }

        public KeyValuePair<int, int> Cell { get; set; }

        public Type SimulationType { get; set; }

        public bool Created { get; set; }
    }
}
