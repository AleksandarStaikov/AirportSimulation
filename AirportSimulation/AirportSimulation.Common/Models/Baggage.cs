﻿namespace AirportSimulation.Common.Models
{
    using System;
    using System.Collections.Generic;

    public class Baggage
    {
        public Baggage()
        {
            Log = new List<BaggageEventLog>();
            FlightNumber = "AD4324SC";
        }

        public BaggageType BaggageType { get; set; }

        public string FlightNumber { get; set; }

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
