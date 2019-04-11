namespace AirportSimulation.Common.Models
{
    using System.Collections.Generic;

    public class Baggage
    {
        public Baggage()
        {
            this.Log = new List<BaggageEventLog>();
        }

        public BaggageType BaggageType { get; set; }

        public string FlightNumber { get; set; }

        public string Owner { get; set; }

        public List<BaggageEventLog> Log { get; set; }
    }
}
