namespace AirportSimulation.Common.Models
{
    using System;

    public class Flight
    {
        public int BaggageCount { get; set; } = 100;

        public TimeSpan TimeToFlightSinceSimulationStart { get; set; } = new TimeSpan(0, 2, 0);

        public TimeSpan TimeToFlightPreparationSinceSimulationStart { get; } = new TimeSpan(0, 1, 0);

        public string FlightNumber { get; set; } = "FR6969";

        public string Gate { get; set; } = "A1";

        public string PickUpArea { get; set; } = "P1";

        public int DispatchedBaggageCount { get; set; } = 0;

        public FlightState FlightState { get; set; } = FlightState.WaitingForPreparation;
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
