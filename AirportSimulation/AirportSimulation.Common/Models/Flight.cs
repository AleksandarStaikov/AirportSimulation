﻿namespace AirportSimulation.Common.Models
{
    using System;

    public class Flight
    {
        public int BaggageCount { get; set; } = 100;

        public TimeSpan TimeToFlightSinceSimulationStart { get; set; } = new TimeSpan(0, 2, 0);

        public string FlightNumber { get; set; } = "FR6969";

        public string Gate { get; set; } = "A1";

        public string PickUpArea { get; set; } = "P1";

        public int DispatchedBaggagesCount { get; set; } = 0;
    }
}
