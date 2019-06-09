namespace AirportSimulation.App.Models
{
    using AirportSimulation.Common;
    using AirportSimulation.Common.Models;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using AirportSimulation.Utility;
    using System.Windows.Input;
    using System;

    internal class FlightsOrganizer
    {
        public static Flight CurrentOutgoingFlight { get; set; } = new Flight();

        public static Flight CurrentIncomingFlight { get; set; } = new Flight();

        public static PscSettings CurrentPscSettings { get; set; } = new PscSettings();

        public static AscSettings CurrentAscSettings { get; set; } = new AscSettings();

        public static ObservableCollection<Flight> OutgoingFlights { get; set; } = new ObservableCollection<Flight>();

        public static ObservableCollection<Flight> IncomingFlights { get; set; } = new ObservableCollection<Flight>();

        public static ObservableCollection<Flight> Flights { get; set; } = new ObservableCollection<Flight>();

        public static int Multiplier { get; set; } = 1;

        public static int TransBaggagePercentage { get; set; }

        public static ICommand GetOutgoingFlightInfo { get; set; } = new RelayCommand<Flight>(AddNewOutgoingFlightCommand);

        public static ICommand GetIncomingFlightInfo { get; set; } = new RelayCommand<Flight>(AddNewIncomingFlightCommand);

        public static ICommand DeleteFlightCommand { get; set; } = new RelayCommand<string>(DeleteFlight);

        private static void DeleteFlight(string flightNumber)
        {
            var flight = Flights.FirstOrDefault(x => x.FlightNumber == flightNumber);

            if (flight == null)
            {
                MessageBox.Show("Flight could not be found!");
                return;
            }

            Flights.Remove(flight);

            switch (flight.FlightState)
            {
                case FlightState.Incoming:
                    IncomingFlights.Remove(flight);
                    break;
                case FlightState.WaitingForPreparation:
                    OutgoingFlights.Remove(flight);
                    break;
            }
        }

        private static void AddNewIncomingFlightCommand(Flight obj)
        {
            var flight = new Flight
            {
                FlightNumber = CurrentIncomingFlight.FlightNumber,
                FlightState = FlightState.Incoming,
                BaggageCount = CurrentIncomingFlight.BaggageCount,
                Gate = CurrentIncomingFlight.Gate,
                PickUpArea = CurrentIncomingFlight.PickUpArea,
                TimeToFlightSinceSimulationStart = CurrentIncomingFlight.TimeToFlightSinceSimulationStart
            };

            if (!FlightWithNumberExists(IncomingFlights, CurrentIncomingFlight.FlightNumber))
            {
                IncomingFlights.Add(flight);
                Flights.Add(flight);
                return;
            }

            MessageBox.Show($"An incoming flight with number {CurrentIncomingFlight.FlightNumber} already exists!");
        }

        private static void AddNewOutgoingFlightCommand(Flight obj)
        {
            var flight = new Flight
            {
                FlightNumber = CurrentOutgoingFlight.FlightNumber,
                FlightState = FlightState.WaitingForPreparation,
                BaggageCount = CurrentOutgoingFlight.BaggageCount,
                Gate = CurrentOutgoingFlight.Gate,
                TimeToFlightSinceSimulationStart = CurrentOutgoingFlight.TimeToFlightSinceSimulationStart,
                TimeToFlightPreparationSinceSimulationStart = CurrentOutgoingFlight.TimeToFlightPreparationSinceSimulationStart
            };

            if (!FlightWithNumberExists(OutgoingFlights, CurrentOutgoingFlight.FlightNumber))
            {
                OutgoingFlights.Add(flight);
                Flights.Add(flight);
                return;
            }

            MessageBox.Show($"An outgoing flight with number {CurrentOutgoingFlight.FlightNumber} already exists!");
        }

        private static bool FlightWithNumberExists(ObservableCollection<Flight> flights, string flightNumber)
        {
            if (!flights.Any())
            {
                return false;
            }

            return flights.Any(x => x.FlightNumber == flightNumber);
        }
    }
}
