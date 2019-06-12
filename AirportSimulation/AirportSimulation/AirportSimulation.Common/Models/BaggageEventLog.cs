namespace AirportSimulation.Common.Models
{
    using System;

    public class BaggageEventLog
    {
        public TimeSpan LogCreated { get; set; }

        public TimeSpan TimeElapsed { get; set; }

        public string Description { get; set; }

        public double? LateForFlight { get; set; }
    }
}
