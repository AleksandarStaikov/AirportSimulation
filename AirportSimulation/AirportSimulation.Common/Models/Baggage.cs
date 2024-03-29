﻿namespace AirportSimulation.Common.Models
{
    using Contracts;
    using System;
    using System.Collections.Generic;

    public class Baggage : IBaggage
    {
        public static List<Baggage> AllBaggage = new List<Baggage>();

        public Baggage()
        {
            Log = new List<BaggageEventLog>();
            Flight = new Flight();

            AllBaggage.Add(this);
        }

        public BaggageType BaggageType { get; set; }

        public IFlight Flight { get; set; }

        public string Destination { get; set; }

        public string Owner { get; set; }

        public List<BaggageEventLog> Log { get; set; }

        public long? TransportationStartTime { get; set; }

        public string TransporterId { get; set; }

        public void AddEventLog(TimeSpan logCreationTime, TimeSpan timeElapsed, string description)
        {
            Log.Add(new BaggageEventLog()
            {
                LogCreated = logCreationTime,
                Description = description, 
                TimeElapsed = timeElapsed 
            });
        }

        public void AddEventLog(TimeSpan logCreationTime, TimeSpan timeElapsed, string description, double lateWithMinutes)
        {
            Log.Add(new BaggageEventLog()
            {
                LogCreated = logCreationTime,
                Description = description, 
                TimeElapsed = timeElapsed,
                LateForFlight = lateWithMinutes
            });
        }
    }
}
