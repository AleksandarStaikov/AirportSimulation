namespace AirportSimulation.Common.Models
{
    using System;
    using System.Collections.Generic;

    public class Baggage
    {
        public Baggage()
        {
            Log = new List<BaggageEventLog>();
            Flight = new Flight();
        }

        public BaggageType BaggageType { get; set; }

        public Flight Flight { get; set; }

        public string Destination { get; set; }

        public string Owner { get; set; }

        public List<BaggageEventLog> Log { get; set; }

        public long? TransportationStartTime { get; set; }

        public void AddEventLog(TimeSpan timeElapsed, string description)
        {
            Log.Add(new BaggageEventLog()
            {
                Description = description, 
                TimeElapsed = timeElapsed 
            });
        }
    }
}
