namespace AirportSimulation.App.Models
{
    using AirportSimulation.Common;
    using AirportSimulation.Common.Models;
    using AirportSimulation.Utility;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    internal class FlightsOrganizer
    {
        public static ObservableCollection<string> FlightStates { get; set; } = 
                FlightStates = typeof(FlightState)
                .GetEnumNames()
                .ToObservableCollection()
                .ToAllowedFlightStates();

        public static Flight CurrentFlight { get; set; } = new Flight();

        public static ObservableCollection<Flight> Flights { get; set; } = new ObservableCollection<Flight>();

        public static List<Flight> GetIncomingFlights() => GetFlightsOfType(FlightState.Incoming);

        public static List<Flight> GetOutgoingFlights() => GetFlightsOfType(FlightState.WaitingForPreparation);

        private static List<Flight> GetFlightsOfType(FlightState state) => Flights.Where(f => f.FlightState == state).ToList();

        public static ICommand GetFlightInfo { get; set; } = new RelayCommand<Flight>(AddNewCommand);

        private static void AddNewCommand(Flight obj)
        {
            var flight = new Flight
            {
                FlightNumber = CurrentFlight.FlightNumber,
                FlightState = CurrentFlight.FlightState,
                BaggageCount = CurrentFlight.BaggageCount,
                DispatchedBaggageCount = CurrentFlight.DispatchedBaggageCount,
                Gate = CurrentFlight.Gate,
                PickUpArea = CurrentFlight.PickUpArea,
                TimeToFlightSinceSimulationStart = CurrentFlight.TimeToFlightSinceSimulationStart
            };

            if (!Flights.Any())
            {
                Flights.Add(flight);
                return;
            }
        
            if (Flights.All(x => x.FlightNumber != flight.FlightNumber))
            {
                Flights.Add(flight);
            }
            else
            {
                MessageBox.Show("A flight with that fligh number already exists!");
            }
        }
    }
}
