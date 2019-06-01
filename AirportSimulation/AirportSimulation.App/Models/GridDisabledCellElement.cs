namespace AirportSimulation.App.Models
{
    using System.Windows;

    internal class GridDisabledCellElement
    {
        public UIElement DisabledElement { get; set; }

        public (int, int) DisabledCell { get; set; }
    }
}
