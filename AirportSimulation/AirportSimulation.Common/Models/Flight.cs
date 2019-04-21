namespace AirportSimulation.Common.Models
{
    using System;

    public class Flight
    {
        public int BaggageCount { get; set; }

        public TimeSpan TimeToFlightSinceSimulationStart { get; set; }

        public string FlightNumber { get; set; }

        public string Gate { get; set; }
    }
}
