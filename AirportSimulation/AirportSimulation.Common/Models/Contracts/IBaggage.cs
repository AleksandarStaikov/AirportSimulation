namespace AirportSimulation.Common.Models.Contracts
{
    using System;
    using System.Collections.Generic;

    public interface IBaggage
    {
        BaggageType BaggageType { get; set; }
        IFlight Flight { get; set; }
        string Destination { get; set; }
        string Owner { get; set; }
        List<BaggageEventLog> Log { get; set; }
        long? TransportationStartTime { get; set; }
        string TransporterId { get; set; }
        void AddEventLog(TimeSpan logCreationTime, TimeSpan timeElapsed, string description);
        void AddEventLog(TimeSpan logCreationTime, TimeSpan timeElapsed, string description, double lateWithMinutes);
    }
}