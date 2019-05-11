namespace AirportSimulation.Utility
{
    using System.Linq;
    using System.Windows.Controls;

    public static class Extensions
    {
        public static Button GetStackPanelChildButton(this StackPanel stackPanel) => stackPanel.Children.OfType<Button>().First();
    }
}
