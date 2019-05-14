namespace AirportSimulation.Utility
{
    using System.Linq;
    using System.Windows.Controls;

    public static class Extensions
    {
        public static Button GetStackPanelChildButton(this StackPanel stackPanel) => stackPanel.Children.OfType<Button>().First();

		public static bool IsCellNull(this (int?, int?) cell) => !cell.Item1.HasValue && !cell.Item2.HasValue;
	}
}
