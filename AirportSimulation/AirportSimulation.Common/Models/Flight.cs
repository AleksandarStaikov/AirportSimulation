namespace AirportSimulation.Common.Models
{
    using System;

    public class Flight
    {
        private TimeSpan _timeToFlightSinceSimulationStart = new TimeSpan(0, 2, 0);
        private TimeSpan _timeToFlightPreparationSinceSimulationStart = new TimeSpan(0, 1, 0);

        public int BaggageCount { get; set; } = 100;

        public TimeSpan TimeToFlightSinceSimulationStart
        {
            get => _timeToFlightSinceSimulationStart;
            set
            {
                _timeToFlightSinceSimulationStart = value;
                //TODO : Add logic ?
                //_timeToFlightPreparationSinceSimulationStart = new TimeSpan(ticks: _timeToFlightSinceSimulationStart.Ticks / ); 
            }
        }

        public string FlightNumber { get; set; } = "FR6969";

        public string Gate { get; set; } = "A1";

        public string PickUpArea { get; set; } = "P1";

        public int DispatchedBaggagesCount { get; set; } = 0;

        public FlightState FlightState { get; set; } = FlightState.WaitingForPreparation;

        public TimeSpan TimeToFlightPreparationSinceSimulationStart => _timeToFlightPreparationSinceSimulationStart; 
    }

    public enum FlightState
    {
        WaitingForPreparation,
        InPreparation, 
        Departed,
        Landed,
        Incoming
    }
}
