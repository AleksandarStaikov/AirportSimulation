namespace AirportSimulation.App.Views
{
    using AirportSimulation.App.Helpers;
    using AirportSimulation.App.Models;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    public partial class FlightsView : UserControl
    {
        public FlightsView()
        {
            InitializeComponent();
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            var data = ConvertToSettingsService.Convert();
            var incomingFlights = FlightsOrganizer.GetIncomingFlights();
            var outgoingFlights = FlightsOrganizer.GetOutgoingFlights();
        }
    }
}
