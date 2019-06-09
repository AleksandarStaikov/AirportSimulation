namespace AirportSimulation.Utility
{
    using AirportSimulation.Common.Models;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Controls;

    public static class Extensions
    {
        public static Button GetStackPanelChildButton(this StackPanel stackPanel) => stackPanel.Children.OfType<Button>().First();

		public static bool IsCellNull(this (int?, int?) cell) => !cell.Item1.HasValue && !cell.Item2.HasValue;

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            var oc = new ObservableCollection<T>();
            collection.ToList().ForEach(x => oc.Add(x));
            return oc;
        }

        public static ObservableCollection<T> ToAllowedFlightStates<T>(this ObservableCollection<T> obCollection)
        {
            var oc = new ObservableCollection<T>();

            foreach (var item in obCollection)
            {
                if (item.Equals(FlightState.Incoming.ToString()) ||
                    item.Equals(FlightState.WaitingForPreparation.ToString()))
                {
                    oc.Add(item);
                }
            }

            return oc;
        }
    }
}
