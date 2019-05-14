namespace AirportSimulation.App.Resources
{
    using AirportSimulation.Common;
    using System.Windows;

    public class GridCell
    {
		public string Id { get; set; }

        public UIElement Element { get; set; }

        public (int, int) Cell { get; set; }

        public BuildingComponentType ElementType { get; set; }

        public bool Created { get; set; }

        public (int?, int?) PreviousCell { get; set; }

        public (int?, int?) NextCell { get; set; }
    }
}
